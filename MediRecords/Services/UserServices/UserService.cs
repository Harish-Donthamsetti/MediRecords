using System;
using System.Diagnostics;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;
using MediRecords.Repository.UserRepo;
using MediRecords.Utility;
using Microsoft.EntityFrameworkCore;

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
    /// <param name="requestDto">The data provided for registration.</param>
    public async Task RegisterUserAsync(UserRegisterRequestDto requestDto)
    {
        // Check if the request (dto) exists
        if (requestDto == null)
        {
            throw new ArgumentException(Constant.RequestNull);
        }

        // Make sure all required information is filled in
        if (string.IsNullOrWhiteSpace(requestDto.Password) ||
           string.IsNullOrWhiteSpace(requestDto.Email) ||
           string.IsNullOrWhiteSpace(requestDto.Name) ||
           requestDto.RoleId <= 0)
        {
            throw new ArgumentException(Constant.RequiredFields);
        }


        // Validate Email
        var emailResult = EmailHelper.ValidateEmail(requestDto.Email);
        if (!emailResult.IsValid)
        {
            throw new MediRecordsException(Constant.InvalidEmailFormat);
        }

        // Validate password
        var passwordResult = PasswordHelper.ValidatePassword(requestDto.Password);
        if (!passwordResult.IsValid)
        {
            throw new MediRecordsException(Constant.WeakPassword);
        }

        // Hash the password to keep it safe in the databse
        requestDto.Password = BCrypt.Net.BCrypt.HashPassword(requestDto.Password);

        // Map the Dto to Domain (User) entity
        var user = requestDto.ToUserRegisterRequest();

        // Pass the data to the repository to get saved
        await _userRepository.RegisterUserAsync(user);
    }
    public async Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request)
    {
        // Check if the request exists
        if (request == null)
            throw new MediRecordsException(Constant.UserUpdate.UpdateUserRequest);
 
        // Validate that the UserID is a positive number
        if (request.UserID <= 0)
            throw new ArgumentException(Constant.UserUpdate.InvalidUserId, nameof(request.UserID));
 
        // Validate that the user's name is provided
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException(Constant.UserUpdate.NameRequired, nameof(request.Name));
 
        // Validate that the phone number is provided
        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException(Constant.UserUpdate.PhoneRequired, nameof(request.Phone));
 
        // Validate that the RoleID is valid
        if (request.RoleID <= 0)
            throw new ArgumentException(Constant.UserUpdate.InvalidRoleId, nameof(request.RoleID));
 
        // Delegate persistence and data update logic to the repository layer
        return await _userRepository.UpdateUser(request);
 
    
    }
 
}
