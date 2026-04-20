using MediRecords.Domain.Entities;
using MediRecords.Dto.PrescriptionItemDtos;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.PrescriptionItemRepository;

public class PrescriptionItemRepository : IPrescriptionItemRepository
{
    private readonly MediRecordsDbContext _context;

    public PrescriptionItemRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PrescriptionItem>> GetAllPrescriptionItemsAsync()
    {
        return await _context.PrescriptionItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PrescriptionItem?> GetPrescriptionItemByIdAsync(int itemId)
    {
        return await _context.PrescriptionItems
            .FirstOrDefaultAsync(pi => pi.ItemId == itemId);
    }

    public async Task<PrescriptionItem> CreatePrescriptionItemAsync(PrescriptionItem item)
    {
        _context.PrescriptionItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<PrescriptionItem> UpdatePrescriptionItemAsync(PrescriptionItem item)
    {
        _context.PrescriptionItems.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeletePrescriptionItemAsync(int itemId)
    {
        var item = await GetPrescriptionItemByIdAsync(itemId);
        if (item == null) return false;

        _context.PrescriptionItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PrescriptionItemExistsAsync(int itemId)
    {
        return await _context.PrescriptionItems.AnyAsync(pi => pi.ItemId == itemId);
    }

    public async Task<bool> PrescriptionExistsAsync(int prescriptionId)
    {
        return await _context.Prescriptions.AnyAsync(p => p.PrescriptionId == prescriptionId);
    }
}