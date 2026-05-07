using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MediRecords.Repository.EncounterRepo;

public class EncounterRepository : IEncounterRepository
{
    private readonly MediRecordsDbContext _context;

    public EncounterRepository(MediRecordsDbContext context)
    {
        _context = context;
    }
    public async Task<Encounter?> GetByIdAsync(int encounterId)
    {
        return await _context.Encounters
            .Include( e => e.PatientIdNavigation)
            .Include( e => e.ProviderIdNavigation)
            .Include ( e => e.SOAPNotes)
            .Include ( e => e.VitalSigns )
            .Include ( e => e.NursingNotes )
            .Include ( e => e.LabOrders )
            .Include ( e => e.Prescriptions )
            .FirstOrDefaultAsync( e => e.EncounterId == encounterId);
    }

    public async Task<IEnumerable<Encounter>> GetByProviderAndDateAsync(int providerId, DateTime date)
    {
        return await _context.Encounters
            .Include(e => e.PatientIdNavigation)
            .Where(e => e.ProviderId == providerId && e.Date.Date == date.Date)
            .OrderBy( e => e.Date)
            .ToListAsync();
    }

    public async Task<Encounter> UpdateStatusAsync(int encounterId, EncounterStatus newStatus)
    {
        var encounter = await _context.Encounters
              .FirstOrDefaultAsync( e => e.EncounterId == encounterId );

        if(encounter == null)
            return null;
        
        encounter.Status = newStatus;
        await _context.SaveChangesAsync();

        return encounter;
    }

    public async Task<int> GetEncounterCountAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Encounters
            .Where(e => e.Date >= fromDate && e.Date <= toDate)
            .CountAsync();
    }

    public async Task<int> GetScheduledAppointmentsCount(int providerId, DateTime startDate, DateTime endDate)
    {
        // Count all appointments in the date range regardless of status —
        // every appointment was "scheduled" at some point, forming the denominator for utilization.
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDateOnly = new DateTime(endDate.Year, endDate.Month, endDate.Day);
        var endDatePlusOne = endDateOnly.AddDays(1);

        return await _context.Appointments
            .Where(a => a.ProviderId == providerId
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne)
            .CountAsync();
    }

    public async Task<int> GetCompletedEncountersCount(int providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDateOnly = new DateTime(endDate.Year, endDate.Month, endDate.Day);
        var endDatePlusOne = endDateOnly.AddDays(1);
        
        return await _context.Appointments
            .Where(a => a.ProviderId == providerId 
                && a.DateTime >= startDateOnly 
                && a.DateTime < endDatePlusOne 
                && a.Status == AppointmentStatus.Completed)
            .CountAsync();
    }

    public async Task<int> GetCancelledEncountersCount(int providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDateOnly = new DateTime(endDate.Year, endDate.Month, endDate.Day);
        var endDatePlusOne = endDateOnly.AddDays(1);

        return await _context.Appointments
            .Where(a => a.ProviderId == providerId
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne
                && a.Status == AppointmentStatus.Cancelled)
            .CountAsync();
    }

    public async Task<int> GetNoShowAppointmentsCount(int providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDateOnly = new DateTime(endDate.Year, endDate.Month, endDate.Day);
        var endDatePlusOne = endDateOnly.AddDays(1);

        return await _context.Appointments
            .Where(a => a.ProviderId == providerId
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne
                && a.Status == AppointmentStatus.NoShow)
            .CountAsync();
    }

    public async Task<int> GetTotalAppointmentsCountAsync(int? providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDatePlusOne = new DateTime(endDate.Year, endDate.Month, endDate.Day).AddDays(1);

        return await _context.Appointments
            .Where(a => (!providerId.HasValue || a.ProviderId == providerId.Value)
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne)
            .CountAsync();
    }

    public async Task<int> GetNoShowCountAsync(int? providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDatePlusOne = new DateTime(endDate.Year, endDate.Month, endDate.Day).AddDays(1);

        return await _context.Appointments
            .Where(a => (!providerId.HasValue || a.ProviderId == providerId.Value)
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne
                && a.Status == AppointmentStatus.NoShow)
            .CountAsync();
    }

    public async Task<int> GetCancellationCountAsync(int? providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDatePlusOne = new DateTime(endDate.Year, endDate.Month, endDate.Day).AddDays(1);

        return await _context.Appointments
            .Where(a => (!providerId.HasValue || a.ProviderId == providerId.Value)
                && a.DateTime >= startDateOnly
                && a.DateTime < endDatePlusOne
                && a.Status == AppointmentStatus.Cancelled)
            .CountAsync();
    }

    public async Task<IEnumerable<Encounter>> GetEncountersForDocumentationReportAsync(int? providerId, DateTime startDate, DateTime endDate)
    {
        var startDateOnly = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        var endDatePlusOne = new DateTime(endDate.Year, endDate.Month, endDate.Day).AddDays(1);

        return await _context.Encounters
            .Include(e => e.SOAPNotes)
            .Include(e => e.ProviderIdNavigation)
            .Where(e => (!providerId.HasValue || e.ProviderId == providerId.Value)
                && e.Date >= startDateOnly
                && e.Date < endDatePlusOne)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }
}