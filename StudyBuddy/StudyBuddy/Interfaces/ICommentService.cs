using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface ICommentService
    {
        Task<PostCommentResponseDTO> PostCommentAsync(PostCommentRequestDTO commentDTO);
        Task<PostReplyResponseDTO> PostReplyAsync(PostReplyRequestDTO replyDTO);
        Task<List<GetCommentsResponseDTO>> GetCommentsAsync(int postId);
        Task<List<GetReplyResponseDTO>> GetRepliesAsync(int commentId);
    }
}
