using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface IReportService
    {
        Task<CreateReportResponseDTO> CreateReportAsync(CreateReportRequestDTO requestDTO);
    }
}
