using Microsoft.AspNetCore.Mvc;
using MediRecords.Services.AuthServices;
using MediRecords.Dto.UserDtos;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginUser(dto);

            if (result == null)
                return Unauthorized(new { message = "Unauthenticated user" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }
}