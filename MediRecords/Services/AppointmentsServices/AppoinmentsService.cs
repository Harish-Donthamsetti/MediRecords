using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.AppointmentsDtos;
using MediRecords.Services.AppointmentsServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AppointmentsService : IAppointmentsService
{
    private readonly MediRecordsDbContext _context;

    public AppointmentsService(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentsResponseDto> BookAppointmentAsync(AppointmentsRequestDto dto)
    {
        if (dto.DateTime < DateTime.Now)
        {
            throw new ArgumentException("You can not book appointment in the past");
        }
        // Guard clauses for required IDs
        if (dto.PatientId <= 0)
            throw new ArgumentException("PatientId is required");

        if (dto.ProviderId <= 0)
            throw new ArgumentException("ProviderId is required");

        // Patient check
        if (!await _context.Patients.AnyAsync(p => p.PatientId == dto.PatientId))
            throw new ArgumentException("Patient not found");

        // Provider check + duration

        var requestedDate = dto.DateTime.Date;

        var providerSchedule = await _context.ProviderSchedules
            .FirstOrDefaultAsync(pr => pr.ProviderId == dto.ProviderId && pr.StartTime.Date == requestedDate);

        Console.WriteLine(providerSchedule);

        if (providerSchedule == null)
            throw new ArgumentException("Provider not found");

        // Slot conflict check (overlap using provider’s duration)
        bool slotTaken = await _context.Appointments.AnyAsync(a =>
            a.ProviderId == dto.ProviderId &&
            a.Status == AppointmentStatus.Booked &&
            dto.DateTime < a.DateTime.AddMinutes(providerSchedule.SlotDuration) &&
            dto.DateTime.AddMinutes(providerSchedule.SlotDuration) > a.DateTime);

        if (slotTaken || providerSchedule.Status == false)
            throw new InvalidOperationException("Provider slot unavailable");

        // Create appointment
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
            Message = "Appointment is booked"
        };
    }


    public async Task<List<AppointmentsResponseDto>> GetAppointmentsAsync(
        int? id,
        int? patientId,
        int? providerId,
        string? date)
    {
        IQueryable<Appointment> query = _context.Appointments.AsNoTracking();

        if (id.HasValue)
            query = query.Where(a => a.AppointmentId == id.Value);

        if (patientId.HasValue)
            query = query.Where(a => a.PatientId == patientId.Value);

        if (providerId.HasValue)
            query = query.Where(a => a.ProviderId == providerId.Value);

        if (!string.IsNullOrEmpty(date))
        {
            // Handle year-only input
            if (int.TryParse(date, out int year))
            {
                query = query.Where(a => a.DateTime.Year == year);
            }
            else if (DateTime.TryParse(date, out DateTime parsed))
            {
                query = query.Where(a =>
                    a.DateTime.Year == parsed.Year &&
                    (parsed.Month > 0 ? a.DateTime.Month == parsed.Month : true) &&
                    (parsed.Day > 0 ? a.DateTime.Day == parsed.Day : true) &&
                    (parsed.Hour > 0 ? a.DateTime.Hour == parsed.Hour : true) &&
                    (parsed.Minute > 0 ? a.DateTime.Minute == parsed.Minute : true)
                );
            }
        }

        var appointments = await query.ToListAsync();
        return appointments.Select(MapToDto).ToList();
    }

    private AppointmentsResponseDto MapToDto(Appointment appointment)
    {
        return new AppointmentsResponseDto
        {
            AppointmentId = appointment.AppointmentId,
            PatientId = appointment.PatientId,
            ProviderId = appointment.ProviderId,
            DateTime = appointment.DateTime,
            Reason = appointment.Reason,
            Status = appointment.Status,
            Message = "Fetched successfully"
        };
    }
}
