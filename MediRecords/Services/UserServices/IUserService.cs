using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;

namespace MediRecords.Services.UserServices;

public interface IUserService
{
    Task RegisterUserAsync(UserRegisterRequestDto requestDto);
    public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
}
