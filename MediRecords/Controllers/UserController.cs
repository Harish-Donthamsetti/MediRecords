using System.Net;
using MediRecords.Dto.UserDtos;
using MediRecords.Services.UserServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Registers a new user into the MediRecords system.
        /// </summary>
        /// <param name="requestDto">The user registration data transfer object containing credentials and profile info.</param>
        /// <returns>Return the Success or ErrorMessage</returns>
        [HttpPost("register")]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestDto requestDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _userService.RegisterUserAsync(requestDto);
                return Ok(Constant.RegisterSuccess);
            }
            catch (MediRecordsException) {
                return StatusCode(500, Constant.InternalError);
            }
        }
    }
}
