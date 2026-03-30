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
    /// <param name="requestDto">The data provided for registration.</param>
    public async Task RegisterUserAsync(UserRegisterRequestDto requestDto)
    {
        // Check if the request (dto) exists
        if (requestDto == null)
        {
            throw new ArgumentException(Constant.RequestNull);
        }

        // Make sure all required information is filled in
        if(string.IsNullOrWhiteSpace(requestDto.Password) ||
           string.IsNullOrWhiteSpace(requestDto.Email) ||
           string.IsNullOrWhiteSpace(requestDto.Name) ||
           requestDto.RoleId <= 0) {
            throw new ArgumentException(Constant.RequiredFields);
           }


        // Validate Email
        var emailResult = EmailHelper.ValidateEmail(requestDto.Email);
        if(!emailResult.IsValid)
        {
            throw new MediRecordsException(Constant.InvalidEmailFormat);
        }

        // Validate password
        var passwordResult = PasswordHelper.ValidatePassword(requestDto.Password);
        if(!passwordResult.IsValid)
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

    /// <summary>
    /// Retrieves all users and transforms the domain models into presentation-ready DTOs.
    /// </summary>
    /// <returns>A collection of UserViewDto objects.</returns>
    public async Task<IEnumerable<UserViewDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(UserViewDto.FromEntity);   
    }

    /// <summary>
    /// Fetches a specific user by ID and converts the entity to a DTO for the API.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The populated UserViewDto if found; otherwise, null.</returns>
    public async Task<UserViewDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        return user != null ? UserViewDto.FromEntity(user) : null;
    }   
}
