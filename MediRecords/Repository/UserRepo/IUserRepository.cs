using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;

namespace MediRecords.Repository.UserRepo;

public interface IUserRepository
{
    Task RegisterUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    
    public Task<UserUpdateResponseDto> UpdateUser(UserUpdateRequestDto request);
}
