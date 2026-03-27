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

    /// <summary>
    /// Checks the user's data, hashes the password, and saves the user to the database.
    /// </summary>
    /// <param name="request">The data provided for registration.</param>
    /// <returns>The result of the registration process.</returns>
    public async Task RegisterUserAsync(UserRegisterRequestDto requestDto)
    {
        // Check if the request (dto) exists
        if (requestDto == null)
        {
            throw new ArgumentException(ErrorMessages.User.RequestNull);
        }

        // Make sure all required information is filled in
        if(string.IsNullOrWhiteSpace(requestDto.Password) ||
           string.IsNullOrWhiteSpace(requestDto.Email) ||
           string.IsNullOrWhiteSpace(requestDto.Name) ||
           requestDto.RoleId <= 0) {
            throw new ArgumentException(ErrorMessages.User.RequiredFields);
           }


        // Validate Email
        var emailResult = EmailHelper.ValidateEmail(requestDto.Email);
        if(!emailResult.IsValid)
        {
            throw new Exception(ErrorMessages.Validation.InvalidEmailFormat);
        }

        // Validate password
        var passwordResult = PasswordHelper.ValidatePassword(requestDto.Password);
        if(!passwordResult.IsValid)
        {
            throw new Exception(ErrorMessages.Validation.WeakPassword);
        }
        
        // Hash the password to keep it safe in the databse
        requestDto.Password = BCrypt.Net.BCrypt.HashPassword(requestDto.Password);

        // Map the Dto to Domain (User) entity
        var user = requestDto.ToUserRegisterRequest();
        
        // Pass the data to the repository to get saved
        await _userRepository.RegisterUserAsync(user);
    }
}
