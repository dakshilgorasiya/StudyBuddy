using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Report> MarkReportAsSolved(int reportId)
        {
            Report report = await _context.Reports.FindAsync(reportId);
            if (report == null)
            {
                return null;
            }
            report.IsSolved = true;
            await _context.SaveChangesAsync();
            report.Owner = _context.Users.FindAsync(report.OwnerId).Result;
            return report;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetAllReportsOfPostAsync(int postId)
        {
            return await _context.Reports
                .Where(r => r.PostId == postId)
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<Report?> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports
                .Include(r => r.Owner)
                .Include(r => r.Post)
                .FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public async Task<bool> IsReportExists(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
            {
                return false;
            }
            return true;
        }
    }
}
