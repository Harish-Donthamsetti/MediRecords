using MediRecords.Dto.UserDtos;
using MediRecords.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterUser(UserRegisterDto dto)
        {
            try {
                await _userService.RegisterUserAsync(dto);
                return Ok(new { Message = "User registered successfully."});
            }
            catch(ArgumentException ex) {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
