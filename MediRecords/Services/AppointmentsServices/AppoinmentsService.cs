using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.AppointmentDtos;
using MediRecords.Dto.AppointmentsDtos;
using MediRecords.Services.AppointmentsServices;
using MediRecords.Utility;

namespace MediRecords.Services.AppointmentsServices
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly MediRecordsDbContext _context;

        public AppointmentsService(MediRecordsDbContext context)
        {
            _context = context;
        }

        // --------------------------------------------------
        // BOOK APPOINTMENT
        // --------------------------------------------------
        public async Task<AppointmentsResponseDto> BookAppointmentAsync(
            AppointmentsRequestDto dto)
        {
            if (dto.DateTime < DateTime.Now)
                throw new ArgumentException(
                    Constant.AppointmentMessages.PastDateNotAllowed);

            if (dto.PatientId <= 0)
                throw new ArgumentException(Constant.PatientIdRequired);

            if (dto.ProviderId <= 0)
                throw new ArgumentException(Constant.ProviderIdRequired);

            bool patientExists = await _context.Patients
                .AnyAsync(p => p.PatientId == dto.PatientId);

            if (!patientExists)
                throw new ArgumentException(Constant.PatientIdRequired);

            bool providerExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.ProviderId);

            if (!providerExists)
                throw new ArgumentException(Constant.ProviderIdRequired);

            var schedule = await _context.ProviderSchedules
                .FirstOrDefaultAsync(ps =>
                    ps.ProviderId == dto.ProviderId &&
                    ps.StartTime.Date == dto.DateTime.Date &&
                    ps.Status == true &&
                    dto.DateTime >= ps.StartTime &&
                    dto.DateTime.AddMinutes(ps.SlotDuration) <= ps.EndTime
                );

            if (schedule == null)
                throw new InvalidOperationException(
                    Constant.AppointmentMessages.ProviderNotAvailable);

            bool slotTaken = await _context.Appointments.AnyAsync(a =>
                a.ProviderId == dto.ProviderId &&
                a.Status == AppointmentStatus.Booked &&
                dto.DateTime < a.DateTime.AddMinutes(schedule.SlotDuration) &&
                dto.DateTime.AddMinutes(schedule.SlotDuration) > a.DateTime
            );

            if (slotTaken)
                throw new InvalidOperationException(
                    Constant.AppointmentMessages.ProviderSlotUnavailable);

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                ProviderId = dto.ProviderId,
                DateTime = dto.DateTime,
                Reason = dto.Reason,
                Status = AppointmentStatus.Booked
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return new AppointmentsResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ProviderId = appointment.ProviderId,
                DateTime = appointment.DateTime,
                Reason = appointment.Reason,
                Status = appointment.Status,
                Message = Constant.Messages.Success
            };
        }

        // --------------------------------------------------
        // GET APPOINTMENTS (FILTERED)
        // --------------------------------------------------
        public async Task<List<AppointmentsResponseDto>> GetAppointmentsAsync(
            int? id,
            int? patientId,
            int? providerId,
            string? date)
        {
            IQueryable<Appointment> query =
                _context.Appointments.AsNoTracking();

            if (id.HasValue)
                query = query.Where(a => a.AppointmentId == id.Value);

            if (patientId.HasValue)
                query = query.Where(a => a.PatientId == patientId.Value);

            if (providerId.HasValue)
                query = query.Where(a => a.ProviderId == providerId.Value);

            if (!string.IsNullOrWhiteSpace(date))
            {
                if (int.TryParse(date, out int year))
                {
                    query = query.Where(a => a.DateTime.Year == year);
                }
                else if (DateTime.TryParse(date, out DateTime parsed))
                {
                    query = query.Where(a =>
                        a.DateTime.Date == parsed.Date);
                }
            }

            var appointments = await query.ToListAsync();

            return appointments.Select(a => new AppointmentsResponseDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                ProviderId = a.ProviderId,
                DateTime = a.DateTime,
                Reason = a.Reason,
                Status = a.Status,
                Message = "Fetched successfully"
            }).ToList();
        }

        // --------------------------------------------------
        // UPDATE APPOINTMENT
        // --------------------------------------------------
        public async Task<AppointmentUpdateResponseDto?> UpdateAppointmentAsync(
            int id,
            AppointmentStatus newStatus,
            AppointmentUpdateRequestDto request)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return null;

            if (appointment.Status == AppointmentStatus.CheckedIn)
                throw new InvalidOperationException(
                    Constant.AppointmentMessages.AppointmentAlreadyCheckedIn);

            // ✅ Patient validation
            if (request.PatientId.HasValue)
            {
                bool patientExists = await _context.Patients
                    .AnyAsync(p => p.PatientId == request.PatientId.Value);

                if (!patientExists)
                    throw new ArgumentException(Constant.PatientIdRequired);

                appointment.PatientId = request.PatientId.Value;
            }

            int providerId = request.ProviderId ?? appointment.ProviderId;
            DateTime appointmentTime =
                request.DateTime ?? appointment.DateTime;

            // ✅ Provider validation
            if (request.ProviderId.HasValue)
            {
                bool providerExists = await _context.Users
                    .AnyAsync(u => u.UserId == request.ProviderId.Value);

                if (!providerExists)
                    throw new ArgumentException(Constant.ProviderIdRequired);

                appointment.ProviderId = request.ProviderId.Value;
            }

            // ✅ Date validation
            if (request.DateTime.HasValue &&
                request.DateTime.Value < DateTime.Now)
                throw new ArgumentException(
                    Constant.AppointmentMessages.PastDateNotAllowed);

            // ✅ Schedule validation
            var schedule = await _context.ProviderSchedules
                .FirstOrDefaultAsync(ps =>
                    ps.ProviderId == providerId &&
                    ps.StartTime.Date == appointmentTime.Date &&
                    ps.Status == true &&
                    appointmentTime >= ps.StartTime &&
                    appointmentTime.AddMinutes(ps.SlotDuration) <= ps.EndTime
                );

            if (schedule == null)
                throw new InvalidOperationException(
                    Constant.AppointmentMessages.ProviderNotAvailable);

            // ✅ Slot conflict
            bool slotTaken = await _context.Appointments.AnyAsync(a =>
                a.AppointmentId != appointment.AppointmentId &&
                a.ProviderId == providerId &&
                a.Status == AppointmentStatus.Booked &&
                appointmentTime < a.DateTime.AddMinutes(schedule.SlotDuration) &&
                appointmentTime.AddMinutes(schedule.SlotDuration) > a.DateTime
            );

            if (slotTaken)
                throw new InvalidOperationException(
                    Constant.AppointmentMessages.ProviderSlotUnavailable);

            appointment.DateTime = appointmentTime;

            if (!string.IsNullOrWhiteSpace(request.Reason))
                appointment.Reason = request.Reason;

            appointment.Status = newStatus;

            await _context.SaveChangesAsync();

            return appointment.ToAppointmentUpdateResponse();
        }
    }
}