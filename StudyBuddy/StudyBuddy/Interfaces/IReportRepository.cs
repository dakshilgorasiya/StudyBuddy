using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> AddReportAsync(Report report);
        Task<Report> MarkReportAsSolved(int reportId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<IEnumerable<Report>> GetAllReportsOfPostAsync(int postId);
        Task<Report?> GetReportByIdAsync(int reportId);
        Task<bool> IsReportExists(int reportId);
    }
}
