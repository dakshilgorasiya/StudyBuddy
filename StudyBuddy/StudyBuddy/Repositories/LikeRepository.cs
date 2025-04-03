using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Like?> ToggleLikeAsync(Like like)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == like.PostId && l.OwnerId == like.OwnerId);
            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return null;
            }
            else
            {
                await _context.Likes.AddAsync(like);
                await _context.SaveChangesAsync();
                return like;
            }
        }

        public async Task<bool> CheckPostLikeAsync(int postId, int ownerId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.OwnerId == ownerId);
            return like != null;
        }

        public async Task<bool> CheckCommentLikeAsync(int commentId, int ownerId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.OwnerId == ownerId);
            return like != null;
        }

        public async Task<int> CountPostLikeAsync(int postId)
        {
            return await _context.Likes
                .CountAsync(l => l.PostId == postId);
        }
        public async Task<int> CountCommentLikeAsync(int commentId)
        {
            return await _context.Likes
                .CountAsync(l => l.CommentId == commentId);
        }
    }
}
