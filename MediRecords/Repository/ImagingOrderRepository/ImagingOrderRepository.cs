using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrderDto;
using MediRecords.Utility;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.ImagingOrderRepository;

public class ImagingOrderRepository : IImagingOrderRepository
{
    private readonly MediRecordsDbContext _context;
    public ImagingOrderRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ImagingOrder order)
    {
        var encounter = await _context.Encounters
            .Select(e => new { e.EncounterId, e.Status }) 
            .FirstOrDefaultAsync(e => e.EncounterId == order.EncounterId);

        if (encounter == null)
        {
            throw new KeyNotFoundException(Constant.EncounterNotFound);
        }

        if (encounter.Status == 0)
        {
            throw new InvalidOperationException(Constant.EncounterClosed);
        }

        await _context.ImagingOrders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ImagingOrder>> GetAllAsync(ImagingOrderFilterDto filter)
    {
        var query = _context.ImagingOrders.AsNoTracking().AsQueryable();

        if (filter.ImagingOrderID.HasValue)
            query = query.Where(x => x.ImagingOrderId == filter.ImagingOrderID);

        if (filter.EncounterID.HasValue)
            query = query.Where(x => x.EncounterId == filter.EncounterID);

        if (!string.IsNullOrWhiteSpace(filter.StudyType))
            query = query.Where(x => x.StudyType.Contains(filter.StudyType));

        if (!string.IsNullOrWhiteSpace(filter.Notes))
            query = query.Where(x => x.Notes.Contains(filter.Notes));

        if (filter.OrderedDate.HasValue)
            query = query.Where(x => x.OrderedDate.Date == filter.OrderedDate.Value.Date);

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status);

        return await query.ToListAsync();
    
    }
}
