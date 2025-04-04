using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;

namespace StudyBuddy.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report> AddReportAsync(Report report)
        {
            _context.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
