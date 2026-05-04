using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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
}