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
    /// Retrieves all users from the database, including their associated role information.
    /// </summary>
    /// <returns>A list of User entities with Role navigation properties populated.</returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        // Use AsNoTracking for "Read-Only" operations to improve performance 
        // and reduce memory usage in Entity Framework.
        return await _context.Users
            .Include(u => u.RoleIdNavigation) 
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Finds a specific user by their unique ID.
    /// </summary>
    /// <param name="id">The primary key ID of the user.</param>
    /// <returns>The User entity if found; otherwise, null.</returns>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.RoleIdNavigation)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    /// <summary>
    /// Handles the database logic for registering a user, including email uniqueness checks and persistence.
    /// </summary>
    /// <param name="user">The registration request containing user details.</param>
    /// <exception cref="Exception">Thrown when a user with the provided email already exists.</exception>
    public async Task RegisterUserAsync(User user)
    {
        if(user == null)
        {
            throw new ArgumentNullException(Constant.RequestNull);
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Status == true);
        if(existingUser != null)
        {
            throw new MediRecordsException(Constant.EmailExists);
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserUpdateResponseDto> UpdateUser(UserUpdateRequestDto request)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserID && request.Status == true);
 
            if (user == null)
            {
                throw new InvalidOperationException(Constant.UserUpdate.UserNotFound);
            }
 
            // Update allowed fields
            user.Name = request.Name;
            user.Phone = request.Phone;
            user.RoleId = request.RoleID;
            user.Status = request.Status;
 
            await _context.SaveChangesAsync();
 
            // Map Domain Entity to Response DTO
            return user.ToUserUpdateResponse();
        }
        catch (ArgumentNullException ex)
        {
            throw new ApplicationException(Constant.UserUpdate.UpdateUserRequest, ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException(Constant.UserUpdate.UserNotFound, ex);
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException(Constant.UserUpdate.UpdateFailed, ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(Constant.InternalError, ex);
        }
    
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync( x => x.Email == email) ?? throw new InvalidOperationException(Constant.UserNotFound);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
