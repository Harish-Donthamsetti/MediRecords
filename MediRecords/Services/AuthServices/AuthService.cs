using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MediRecords.Domain.Entities;
using MediRecords.Dto.UserDtos;
using MediRecords.Repositories;
using BCrypt.Net; 

namespace MediRecords.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepo;
    private readonly IConfiguration _config;

    public AuthService(IAuthRepository authRepo, IConfiguration config)
    {
        _authRepo = authRepo;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginUser(LoginRequestDto dto)
    {
        var user = await _authRepo.GetUserByEmailAsync(dto.Email);
        
        // If user not found or password doesn't match BCrypt format/value
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) 
        {
            return null;
        }

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        await SaveAuditLog(user.UserId, "Login");

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task SaveAuditLog(int userId, string action)
    {
        await _authRepo.AddAuditLogAsync(new AuditLog
        {
            UserId = userId,
            Action = action
        });
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var keyStr = _config["Jwt:Key"] ?? "SecretKeyWithAtLeast32CharactersLong123!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}