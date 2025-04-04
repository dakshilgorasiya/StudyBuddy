using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface IAdminService
    {
        Task<List<GetAllReportsReponseDTO>> GetAllReportsAsync();
        Task<List<GetAllReportsOfPostResponseDTO>> GetAllReportsOfPostAsync(int PostId);
        Task<MarkReportAsSolvedResponseDTO> MarkReportAsSolvedAsync(MarkReportAsSolvedRequestDTO request);
    }
}
