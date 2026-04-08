using MediRecords.Domain.Entities;
using MediRecords.Dto.AppointmentsDtos;
using MediRecords.Services.AppointmentsServices;
using Microsoft.EntityFrameworkCore;

public class AppointmentsService : IAppointmentsService
{
    private readonly MediRecordsDbContext _context;

    public AppointmentsService(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<int> BookAppointmentAsync(AppointmentsRequestDto dto)
    {
        // 🔹 Verify patient exists
        bool patientExists = await _context.Patients.AnyAsync(p => p.PatientId == dto.PatientId);
        if (!patientExists)
        {
            throw new ArgumentException("Patient not found");
        }

        // Verify provider exists
        bool providerExists = await _context.ProviderSchedules.AnyAsync(pr => pr.ProviderId == dto.ProviderId);
        if (!providerExists)
        {
            throw new ArgumentException("Provider not found");
        }

        // Check provider availability (same provider, same time)
        bool slotExists = await _context.Appointments.AnyAsync(a =>
            a.ProviderId == dto.ProviderId &&
            a.DateTime == dto.DateTime &&
            a.Status == true);

        if (slotExists)
        {
            throw new InvalidOperationException("Provider slot unavailable");
        }

        // Create Appointment entity
        Appointment appointment = new Appointment
        {
            PatientId = dto.PatientId,
            ProviderId = dto.ProviderId,
            DateTime = dto.DateTime,
            Reason = dto.Reason,
            Status = true
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return appointment.AppointmentId;
    }
}
