using System;
using MediRecords.Domain.Entities;
using MediRecords.Utility;
using MediRecords.Dto.UserDtos;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly MediRecordsDbContext _context;
    public UserRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the database logic for registering a user, including email uniqueness checks and persistence.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>A response DTO containing the mapped details of the newly created user.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="Exception">Thrown when a user with the provided email already exists.</exception>
    public async Task RegisterUserAsync(User user)
    {
        try
        {
            if(user == null)
            {
                throw new ArgumentNullException(ErrorMessages.User.RequestNull);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(existingUser != null)
            {
                throw new Exception(ErrorMessages.User.EmailExists);
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch(Exception)
        {
            throw new Exception(ErrorMessages.Database.SaveFailed);
        }
    }
}
