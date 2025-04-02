using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudyBuddy.Interfaces;
using StudyBuddy.DTOs;
using StudyBuddy.Common;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost("toggleFollow")]
        [Authorize]
        public async Task<IActionResult> ToggleFollow([FromBody] FollowUserRequestDTO followDTO)
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

                string message = await _followService.FollowUserAsync(followDTO);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<FollowUserResponseDTO>(201, message, null));
            }
            catch (ErrorResponse ex)
            {
                return StatusCode(ex.StatusCode, new { ex.StatusCode, ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse(500, "An error occurred while processing your request."));
            }
        }

        [HttpGet("checkFollowing/{id}")]
        [Authorize]
        public async Task<IActionResult> CheckFollow(int id)
        {
            try
            {
                var response = await _followService.CheckFollowingAsync(id);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<CheckFollowingResponseDTO>(200, "Check following status", response));
            }
            catch (ErrorResponse ex)
            {
                return StatusCode(ex.StatusCode, new { ex.StatusCode, ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse(500, "An error occurred while processing your request."));
            }
        }

        [HttpGet("getFollowers/{id}")]
        public async Task<IActionResult> GetFollowers(int id)
        {
            try
            {
                var response = await _followService.GetFollowersAsync(id);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetFollowersResponseDTO>(200, "Get followers", response));
            }
            catch (ErrorResponse ex)
            {
                return StatusCode(ex.StatusCode, new { ex.StatusCode, ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse(500, "An error occurred while processing your request."));
            }
        }

        [HttpGet("getFollowing/{id}")]
        public async Task<IActionResult> GetFollowing(int id)
        {
            try
            {
                var response = await _followService.GetFollowingAsync(id);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetFollowingResponseDTO>(200, "Get following", response));
            }
            catch (ErrorResponse ex)
            {
                return StatusCode(ex.StatusCode, new { ex.StatusCode, ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse(500, "An error occurred while processing your request."));
            }
        }
    }
}
