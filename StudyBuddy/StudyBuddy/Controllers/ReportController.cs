using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Interfaces;
using StudyBuddy.DTOs;
using Microsoft.AspNetCore.Authorization;
using StudyBuddy.Common;
using StudyBuddy.Services;

namespace StudyBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("createReport")]
        [Authorize]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestDTO report)
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
                CreateReportResponseDTO response = await _reportService.CreateReportAsync(report);
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateReportResponseDTO>(201, "Report created successfully", response));
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
