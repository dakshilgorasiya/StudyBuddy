using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class LikePostRequestDTO
    {
        [Required(ErrorMessage = "PostId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PostId must be a positive integer")]
        public int PostId { get; set; }
    }

    public class LikePostResponseDTO
    {

    }

    public class LikeCommentRequestDTO
    {
        [Required(ErrorMessage = "CommentId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CommentId must be a positive integer")]
        public int CommentId { get; set; }
    }

    public class LikeCommentResponseDTO
    {

    }

    public class GetPostLikesResponseDTO
    {
        public int LikesCount { get; set; }
    }

    public class GetCommentLikesResponseDTO
    {
        public int LikesCount { get; set; }
    }

    public class CheckPostLikeResponseDTO
    {
        public bool IsLiked { get; set; }
    }

    public class CheckCommentLikeResponseDTO
    {
        public bool IsLiked { get; set; }
    }
}
