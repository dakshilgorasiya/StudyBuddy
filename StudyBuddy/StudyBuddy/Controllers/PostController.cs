using Microsoft.AspNetCore.Mvc;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using Microsoft.AspNetCore.Authorization;
using StudyBuddy.Common;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("createPost")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostRequestDTO createPostDto)
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

                var post = await _postService.CreatePostAsync(createPostDto);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreatePostResponceDTO>(201, "Post created successfully", post));  // Success response
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

        [HttpGet("getAllPosts")]
        public async Task<IActionResult> GetAllPosts(int page, int pagesize)
        {
            try
            {
                GetAllPostsResponseDTO posts = await _postService.GetAllPostsAsync(page, pagesize);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetAllPostsResponseDTO>(200, "Posts retrieved successfully", posts));  // Success response
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

        [HttpGet("getPostById/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                GetPostByIdResponseDTO post = await _postService.GetPostByIdAsync(postId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<GetPostByIdResponseDTO>(200, "Post retrieved successfully", post));  // Success response
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
