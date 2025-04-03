using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> AddCommentAsync(Comment comment);
        Task<bool> IsCommentExists(int id);
        Task<IEnumerable<Comment>> GetAllCommentsByPostId(int postId);
        Task<IEnumerable<Comment>> GetAllRepliesByCommentId(int commentId);
        Task<int> CountComments(int postId);
        Task<int> CountReply(int commentId);
    }
}
