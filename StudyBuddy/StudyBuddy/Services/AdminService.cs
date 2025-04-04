using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using StudyBuddy.DTOs;
using AutoMapper;
using StudyBuddy.Common;

namespace StudyBuddy.Services
{
    public class AdminService : IAdminService
    {
        private readonly IPostRepository _postRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(IPostRepository postRepository, IReportRepository reportRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetAllReportsReponseDTO>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllReportsAsync();
            return _mapper.Map<List<GetAllReportsReponseDTO>>(reports);
        }

        public async Task<List<GetAllReportsOfPostResponseDTO>> GetAllReportsOfPostAsync(int PostId)
        {
            bool IsPostExits = await _postRepository.IsPostExists(PostId);

            if(!IsPostExits)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }

            Console.WriteLine("HI");

            var reports = await _reportRepository.GetAllReportsOfPostAsync(PostId);
            return _mapper.Map<List<GetAllReportsOfPostResponseDTO>>(reports);
        }

        public async Task<MarkReportAsSolvedResponseDTO> MarkReportAsSolvedAsync(MarkReportAsSolvedRequestDTO request)
        {
            bool IsReportExist = await _reportRepository.IsReportExists(request.Id);
            if (!IsReportExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Report not found");
            }
            Report report = await _reportRepository.MarkReportAsSolved(request.Id);
            return _mapper.Map<MarkReportAsSolvedResponseDTO>(report);
        }
    }
}
