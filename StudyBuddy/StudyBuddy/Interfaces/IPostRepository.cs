using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<bool> IsPostExists(int postId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostById(int postId);
    }
}
