using System;
using MediRecords.Domain.Entities;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Repository.ImmunizationRepository;

public class ImmunizationRepository : IImmunizationRepository
{
    private readonly MediRecordsDbContext _context;
    public ImmunizationRepository(MediRecordsDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Immunization immunization)
    {
        await _context.Immunizations.AddAsync(immunization);
        await _context.SaveChangesAsync(); 
    }
}
