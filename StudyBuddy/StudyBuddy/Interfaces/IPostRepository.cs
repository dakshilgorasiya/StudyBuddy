using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<bool> IsPostExists(int postId);
        Task<IEnumerable<Post>> GetAllPostsAsync(int page, int pagesize);
        Task<Post?> GetPostById(int postId);
        Task<int> CountPost();
    }
}
