using AutoMapper;
using StudyBuddy.Interfaces;
using StudyBuddy.DTOs;
using System.Security.Claims;
using StudyBuddy.Common;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(
            IMapper mapper,
            IReportRepository reportRepository,
            IPostRepository postRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateReportResponseDTO> CreateReportAsync(CreateReportRequestDTO requestDTO)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User not authenticated");
            }
            bool post = await _postRepository.IsPostExists(requestDTO.PostId);
            if (!post)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }
            Report report = _mapper.Map<Report>(requestDTO);
            report.OwnerId = int.Parse(userId);
            Report createdReport = await _reportRepository.AddReportAsync(report);
            return _mapper.Map<CreateReportResponseDTO>(createdReport);
        }
    }
}
