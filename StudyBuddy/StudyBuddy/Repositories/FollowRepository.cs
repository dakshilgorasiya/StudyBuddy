using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext _context;

        public FollowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Follow?> ToggleFollowAsync(Follow follow)
        {
            var existingFollow = await GetFollowAsync(follow.FollowedById, follow.FollowedToId);
            if (existingFollow != null)
            {
                _context.Follows.Remove(existingFollow);
                await _context.SaveChangesAsync();
                return null;
            }
            else
            {
                _context.Follows.Add(follow);
                await _context.SaveChangesAsync();
                return follow;
            }
        }
        public async Task<Follow?> GetFollowAsync(int followedById, int followedToId)
        {
            return await _context.Follows.FirstOrDefaultAsync(f => f.FollowedById == followedById && f.FollowedToId == followedToId);
        }
        public async Task<int> GetFollowersAsync(int id)
        {
            return await _context.Follows.CountAsync(f => f.FollowedToId == id);
        }
        public async Task<int> GetFollowingAsync(int id)
        {
            return await _context.Follows.CountAsync(f => f.FollowedById == id);
        }
    }
}
