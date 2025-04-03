using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like?> ToggleLikeAsync(Like like);
        Task<bool> CheckPostLikeAsync(int postId, int ownerId);
        Task<bool> CheckCommentLikeAsync(int commentId, int ownerId);
        Task<int> CountPostLikeAsync(int postId);
        Task<int> CountCommentLikeAsync(int commentId);
    }
}
