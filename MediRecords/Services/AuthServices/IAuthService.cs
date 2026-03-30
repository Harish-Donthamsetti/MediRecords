using System;
using MediRecords.Dto.UserDtos;

namespace MediRecords.Services.AuthService;

public interface IAuthService
{
    Task <(bool Success, string Message)> ForgotPasswordAsync(UserForgotPasswordDto model);
}