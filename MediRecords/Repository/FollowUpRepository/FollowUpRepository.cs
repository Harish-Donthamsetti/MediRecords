using System;
using MediRecords.Domain.Entities;
using MediRecords.Utility;
using Microsoft.IdentityModel.Tokens;

namespace MediRecords.Repository.FollowUpRepository;

public class FollowUpRepository : IFollowUpRepository
{
    private  readonly MediRecordsDbContext _context;
    public FollowUpRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task AddFollowUpAsync(FollowUp followUp)
    {
        await _context.FollowUps.AddAsync(followUp);
        await _context.SaveChangesAsync();
    }
}
