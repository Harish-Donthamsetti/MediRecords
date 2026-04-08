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
        // Patient check
        if (!await _context.Patients.AnyAsync(p => p.PatientId == dto.PatientId))
            throw new ArgumentException("Patient not found");

        // Provider check
        if (!await _context.ProviderSchedules.AnyAsync(pr => pr.ProviderId == dto.ProviderId))
            throw new ArgumentException("Provider not found");

        // Slot conflict check
        bool slotTaken = await _context.Appointments.AnyAsync(a =>
            a.ProviderId == dto.ProviderId &&
            a.DateTime == dto.DateTime &&
            a.Status == AppointmentStatus.Booked);

        if (slotTaken)
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
