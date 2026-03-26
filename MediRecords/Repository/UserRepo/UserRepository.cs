using System;
using MediRecords.Domain.Entities;
using MediRecords.Utility;

namespace MediRecords.Repository.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly MediRecordsDbContext _context;
    public UserRepository(MediRecordsDbContext context)
    {
        _context = context;
    }
    public async Task RegisterUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
