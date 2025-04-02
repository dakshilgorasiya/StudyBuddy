using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Common;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .FirstOrDefault() ?? "Invalid request";

                    throw new ErrorResponse(StatusCodes.Status400BadRequest, errorMessage);
                }

                var user = await _userService.RegisterUser(dto);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<UserRegisterResponseDTO>(201, "User created successfully", user));  // Success response
            }
            catch (ErrorResponse ex)
            {
                var errorResponse = new { ex.StatusCode, ex.Message }; // Avoid serializing the entire exception
                return StatusCode(ex.StatusCode, errorResponse);
            }
            catch (Exception)
            {
                var errorResponse = new ErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                return StatusCode(500, errorResponse);  // Return 500 error for unexpected issues
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .FirstOrDefault() ?? "Invalid request";

                    throw new ErrorResponse(StatusCodes.Status400BadRequest, errorMessage);
                }

                var response = await _userService.LoginUser(dto);

                if (response == null)
                {
                    throw new ErrorResponse(StatusCodes.Status401Unauthorized, "Invalid credentials");
                }

                return StatusCode(StatusCodes.Status200OK, new ApiResponse<UserLoginResponseDTO>(200, "Login successful", response));
            }
            catch (ErrorResponse ex)
            {
                var errorResponse = new { ex.StatusCode, ex.Message }; // Avoid serializing the entire exception
                return StatusCode(ex.StatusCode, errorResponse);
            }
            catch (Exception)
            {
                var errorResponse = new ErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                return StatusCode(500, errorResponse);  // Return 500 error for unexpected issues
            }
        }

        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var response = await _userService.GetCurrentUser();
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<UserGetCurrentResponseDTO>(200, "User retrieved successfully", response));
            }
            catch (ErrorResponse ex)
            {
                var errorResponse = new { ex.StatusCode, ex.Message }; // Avoid serializing the entire exception
                return StatusCode(ex.StatusCode, errorResponse);
            }
            catch (Exception)
            {
                var errorResponse = new ErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                return StatusCode(500, errorResponse);  // Return 500 error for unexpected issues
            }
        }
    }
}
