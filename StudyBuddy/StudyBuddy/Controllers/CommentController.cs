using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Interfaces;
using StudyBuddy.DTOs;
using StudyBuddy.Common;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("postComment")]
        [Authorize]
        public async Task<IActionResult> PostComment([FromBody] PostCommentRequestDTO commentDTO)
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
                var response = await _commentService.PostCommentAsync(commentDTO);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<PostCommentResponseDTO>(201, "Comment posted successfully", response));
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

        [HttpPost("postReply")]
        [Authorize]
        public async Task<IActionResult> PostReply([FromBody] PostReplyRequestDTO replyDTO)
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
                var response = await _commentService.PostReplyAsync(replyDTO);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<PostReplyResponseDTO>(201, "Reply posted successfully", response));
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

        [HttpGet("getComments/{postId}")]
        public async Task<IActionResult> GetComments(int postId)
        {
            try
            {
                var response = await _commentService.GetCommentsAsync(postId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<List<GetCommentsResponseDTO>>(200, "Comments retrieved successfully", response));
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

        [HttpGet("getReplies/{commentId}")]
        public async Task<IActionResult> GetReplies(int commentId)
        {
            try
            {
                var response = await _commentService.GetRepliesAsync(commentId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<List<GetReplyResponseDTO>>(200, "Replies retrieved successfully", response));
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
