using System;
using Microsoft.EntityFrameworkCore;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.BillingRepo;

public class BillingRepository : IBillingRepository
{
    private readonly MediRecordsDbContext _context;
    public BillingRepository(MediRecordsDbContext context)
    {
        _context = context;
    }
    public async Task<VisitChargeRef> AddAsync(VisitChargeRef charge)
    {
        await _context.VisitChargeRefs.AddAsync(charge);
        await _context.SaveChangesAsync();

        // Reload with navigation properties for response
        return await _context.VisitChargeRefs
            .Include(v => v.ProcedureCodeNavigation)
            .FirstAsync(v => v.ChargeId == charge.ChargeId);
    }

    public async Task<bool> ChargeExistsAsync(int encounterId, int codeId)
    {
        return await _context.VisitChargeRefs.AnyAsync(v => v.EncounterId == encounterId && v.CodeId == codeId);
    }

    public async Task<bool> EncounterExistsAsync(int encounterId)
    {
        return await _context.Encounters.AnyAsync(e => e.EncounterId == encounterId);
    }

    public async Task<ProcedureCode?> GetProcedureCodeByIdAsync(int codeId)
    {
        return await _context.ProcedureCodes.FirstOrDefaultAsync(p => p.CodeId == codeId);
    }

    public async Task<VisitChargeRef?> GetChargeByIdAsync(int chargeId)
    {
        return await _context.VisitChargeRefs
            .Include(v => v.ProcedureCodeNavigation)
            .FirstOrDefaultAsync(v => v.ChargeId == chargeId);
    }

    public async Task<VisitChargeRef> UpdateAsync(VisitChargeRef charge)
    {
        _context.VisitChargeRefs.Update(charge);
        await _context.SaveChangesAsync();
        return charge;
    }

    public async Task<IEnumerable<VisitChargeRef>> GetChargesByEncounterIdAsync(int encounterId)
    {
        return await _context.VisitChargeRefs
            .Include(v => v.ProcedureCodeNavigation)
            .Where(v => v.EncounterId == encounterId)
            .OrderBy(v => v.ChargeId)
            .ToListAsync();
    }
}