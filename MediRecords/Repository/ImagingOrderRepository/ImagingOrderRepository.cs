using System;
using MediRecords.Domain.Entities;
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

    /// <summary>
    /// Validates the existence and status of an encounter before asynchronously adding a new imaging order.
    /// </summary>
    /// <param name="order">The imaging order entity to be added.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the associated encounter does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the encounter is closed (Status is 0).</exception>
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
}
