using System;
using System.Diagnostics;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;
using MediRecords.Repository.UserRepo;
using MediRecords.Utility;

namespace MediRecords.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task RegisterUserAsync(UserRegisterDto dto)
    {
        // Basic validations
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required");

        // Validate password
        PasswordValidator.Validate(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            RoleId = dto.RoleId,
            Email = dto.Email,
            Phone = dto.Phone ?? "",
            Password = PasswordHasher.HashPassword(dto.Password),
            Status = dto.Status
        };

        return _userRepository.RegisterUserAsync(user);
    }
}
