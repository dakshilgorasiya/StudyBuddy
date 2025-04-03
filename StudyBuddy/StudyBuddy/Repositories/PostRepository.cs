using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;

namespace StudyBuddy.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> IsPostExists(int postId)
        {
            return await _context.Posts.AnyAsync(p => p.Id == postId);
        }
    }
}
