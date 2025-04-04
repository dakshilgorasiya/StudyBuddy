using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> AddReportAsync(Report report);
    }
}
