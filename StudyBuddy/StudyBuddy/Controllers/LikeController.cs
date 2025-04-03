using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Interfaces;
using StudyBuddy.DTOs;
using StudyBuddy.Common;
using Microsoft.AspNetCore.Authorization;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("likePost")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromBody] LikePostRequestDTO likeDTO)
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
                string response = await _likeService.LikePostAsync(likeDTO);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<LikePostResponseDTO>(201, response, null));
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

        [HttpPost("likeComment")]
        [Authorize]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequestDTO likeDTO)
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
                string response = await _likeService.LikeCommentAsync(likeDTO);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<LikePostResponseDTO>(201, response, null));
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

        [HttpGet("getPostLikes/{postId}")]
        public async Task<IActionResult> GetPostLikes(int postId)
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
                GetPostLikesResponseDTO response = await _likeService.GetPostLikesAsync(postId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetPostLikesResponseDTO>(200, "Post likes fetched", response));
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

        [HttpGet("getCommentLikes/{commentId}")]
        public async Task<IActionResult> GetCommentLikes(int commentId)
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
                GetCommentLikesResponseDTO response = await _likeService.GetCommentLikesAsync(commentId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetCommentLikesResponseDTO>(200, "Comment likes fetched", response));
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

        [HttpGet("checkPostLike/{postId}")]
        [Authorize]
        public async Task<IActionResult> CheckPostLike(int postId)
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
                CheckPostLikeResponseDTO response = await _likeService.CheckPostLikeAsync(postId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<CheckPostLikeResponseDTO>(200, "Post likes fetched", response));
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

        [HttpGet("checkCommentLike/{commentId}")]
        [Authorize]
        public async Task<IActionResult> CheckCommentLike(int commentId)
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
                CheckCommentLikeResponseDTO response = await _likeService.CheckCommentLikeAsync(commentId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<CheckCommentLikeResponseDTO>(200, "Comment likes fetched", response));
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
