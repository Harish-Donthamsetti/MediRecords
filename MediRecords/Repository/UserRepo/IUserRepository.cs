using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;

namespace MediRecords.Repository.UserRepo;

public interface IUserRepository
{
    Task RegisterUserAsync(User user);
    Task<User> GetByEmailAsync(string email);
    Task UpdateAsync(User user);
}
