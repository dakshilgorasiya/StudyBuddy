using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Interfaces;
using Microsoft.AspNetCore.Authorization;
using StudyBuddy.DTOs;
using StudyBuddy.Common;
using StudyBuddy.Services;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("getAllReports")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                List<GetAllReportsReponseDTO> reports = await _adminService.GetAllReportsAsync();
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<List<GetAllReportsReponseDTO>>(200, "Reports fetched successfully", reports));
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

        [HttpGet("getAllReportsOfPost/{PostId}")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetAllReportsOfPost(int PostId)
        {
            try
            {
                List<GetAllReportsOfPostResponseDTO> reports = await _adminService.GetAllReportsOfPostAsync(PostId);
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<List<GetAllReportsOfPostResponseDTO>>(200, "Reports fetched successfully", reports));
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

        [HttpPatch("markReportAsSolved")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> MarkReportAsSolved([FromBody] MarkReportAsSolvedRequestDTO request)
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
                MarkReportAsSolvedResponseDTO report = await _adminService.MarkReportAsSolvedAsync(request);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<MarkReportAsSolvedResponseDTO>(200, "Report marked as solved", report));
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
