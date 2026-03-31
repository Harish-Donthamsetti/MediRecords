using System.ComponentModel.DataAnnotations;
using MediRecords.Utility;

namespace MediRecords.Dto.UserDtos;

public class UserForgotPasswordDto
{
    [Required(ErrorMessage = Messages.EmailRequired)]
    [EmailAddress(ErrorMessage = Messages.EmailInvalid)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.NewPasswordRequired)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.ConfirmPasswordRequired)]
    public string ConfirmPassword { get; set; } = string.Empty;
}