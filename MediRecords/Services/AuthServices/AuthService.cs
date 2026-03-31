using System.Text.RegularExpressions;
using MediRecords.Dto.UserDtos;
using MediRecords.Repository.UserRepo;
using MediRecords.Utility;

namespace MediRecords.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool Success, string Message)> ForgotPasswordAsync(UserForgotPasswordDto model)
    {
        try
        {
            // Validate password match
            if (model.NewPassword != model.ConfirmPassword)
                return (false,  Messages.PasswordMismatch);

            // Validate password strength
            if (!IsValidPassword(model.NewPassword))
                return (false, Messages.WeakPassword);

            // Check user exists
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, Messages.UserNotFound);

            // Hash and update password
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _userRepository.UpdateAsync(user);

            return (true,  Messages.PasswordUpdated);
        }
        catch (Exception ex)
        {
            // Log ex here if a logger is injected (recommended)
            return (false, Messages.SomethingWentWrong);
        }
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        // Min 8 chars, at least one uppercase, one lowercase, one digit, one special char
        var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        return Regex.IsMatch(password, pattern);
    }
}