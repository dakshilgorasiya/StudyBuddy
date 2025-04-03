using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> IsCommentExists(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByPostId(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Owner)
                .Include(c => c.Replies)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllRepliesByCommentId(int commentId)
        {
            return await _context.Comments
                .Where(c => c.CommentId == commentId)
                .Include(c => c.Owner)
                .Include(c => c.Replies)
                .ToListAsync();
        }

        public async Task<int> CountComments(int postId)
        {
            return await _context.Comments.CountAsync(c => c.PostId == postId);
        }
        public async Task<int> CountReply(int commentId)
        {
            return await _context.Comments.CountAsync(c => c.CommentId == commentId);
        }
    }
}
