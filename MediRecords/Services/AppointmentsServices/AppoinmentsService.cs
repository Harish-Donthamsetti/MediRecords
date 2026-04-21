using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.AppointmentsDtos;
using MediRecords.Services.AppointmentsServices;
using Microsoft.EntityFrameworkCore;
using System;
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
        if(dto.DateTime < DateTime.Now)
        {
            throw new ArgumentException("You can not book appointment in the past");
        }
        // Guard clauses for required IDs
        if (dto.PatientId <= 0)
            throw new ArgumentException("PatientId is required");

        if (dto.ProviderId <= 0)
            throw new ArgumentException("ProviderId is required");

        // Patient check
        if (!await _context.Patients.AnyAsync(p => p.PatientId == dto.PatientId && p.Status == 0))
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
}
