using MediRecords.Models;
using MediRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.VitalSignRepository;

public class VitalSignRepository : IVitalSignRepository
{
    private readonly MediRecordsDbContext _context;

    public VitalSignRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<VitalSign> AddAsync(VitalSign vitalSign)
    {
        if (vitalSign.VitalId == 0)
        {
            vitalSign.VitalId = await GetNextVitalIdAsync();
        }
        _context.VitalSign.Add(vitalSign);
        await _context.SaveChangesAsync();
        return vitalSign;
    }

    public async Task<int> GetNextVitalIdAsync()
    {
        var maxId = await _context.VitalSign.MaxAsync(x => (int?)x.VitalId) ?? 0;
        return maxId + 1;
    }

    public async Task<Encounter?> GetEncounterByIdAsync(int encounterId)
    {
        return await _context.Encounters.FindAsync(encounterId);
    }
}