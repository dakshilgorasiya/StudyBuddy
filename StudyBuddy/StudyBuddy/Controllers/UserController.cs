using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Common;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;

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
                return Ok(user);  // Success response
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
