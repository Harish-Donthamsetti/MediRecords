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
    /// <param name="user">The registration request containing user details.</param>
    /// <exception cref="Exception">Thrown when a user with the provided email already exists.</exception>
    public async Task RegisterUserAsync(User user)
    {
        try
        {
            if (user == null)
            {
                throw new ArgumentNullException(Constant.RequestNull);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Status == true);
            if (existingUser != null)
            {
                throw new MediRecordsException(Constant.EmailExists);
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new MediRecordsException(Constant.SaveFailed);
        }
    }

    public async Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserID);
 
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
            return user.ToUserUpdateByAdminResponse();
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
            throw new DbUpdateException(Constant.Database.UpdateFailed, ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(Constant.User.InternalError, ex);
        }
    
    }
}
