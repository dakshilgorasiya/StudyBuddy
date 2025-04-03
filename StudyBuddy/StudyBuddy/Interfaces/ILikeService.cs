using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface ILikeService
    {
        Task<string> LikePostAsync(LikePostRequestDTO likedto);
        Task<string> LikeCommentAsync(LikeCommentRequestDTO likedto);
        Task<GetPostLikesResponseDTO> GetPostLikesAsync(int postId);
        Task<GetCommentLikesResponseDTO> GetCommentLikesAsync(int commentId);
        Task<CheckPostLikeResponseDTO> CheckPostLikeAsync(int postId);
        Task<CheckCommentLikeResponseDTO> CheckCommentLikeAsync(int commentId);
    }
}
