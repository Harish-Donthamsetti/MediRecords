using System.Net;
using MediRecords.Dto.UserDtos;
using MediRecords.Services.UserServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Services.AuthService;

namespace MediRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        // Combine both services into one constructor
        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _userService.RegisterUserAsync(requestDto);
                return Ok(Constant.RegisterSuccess);
            }
            catch (MediRecordsException)
            {
                return StatusCode(500, Constant.InternalError);
            }
        }

        /// <summary>
        /// Retrieves a list of all registered users.
        /// </summary>
        /// <returns>A collection of UserViewDto objects.</returns>
        // [Authorize(Roles = Constant.Admin)]
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserViewDto>>> GetAll()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = Constant.InternalError});
            }
        }

        /// <summary>
        /// Fetches a specific user's details by their unique ID.
        /// </summary>
        /// <param name="id">The numeric ID of the user.</param>
        // [Authorize(Roles = Constant.Admin)]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserViewDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = Constant.InvalidUserId});
            }

            try
            {
                var result = await _userService.GetUserByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = string.Format(Constant.UserNotFound, id) });
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = Constant.InternalError});
            }
        }

        [HttpPost("forgotpassword")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserForgotPassword([FromBody] UserForgotPasswordDto model)
        {
            if (model == null)
                return BadRequest(Messages.InvalidRequest);

            var (success, message) = await _authService.ForgotPasswordAsync(model);

            if (!success)
                return BadRequest(message);

            return Ok(new { message });
        }
    }
}