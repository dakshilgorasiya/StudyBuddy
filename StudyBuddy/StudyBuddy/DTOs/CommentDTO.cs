using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class PostCommentRequestDTO 
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
    public class PostCommentResponseDTO 
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
    }
    public class PostReplyRequestDTO 
    {
        public string Content { get; set; }
        public int CommentId { get; set; }
    }
    public class PostReplyResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CommentId { get; set; }
    }
    public class GetCommentsResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public UserDTO User { get; set; }
        public int ReplyCount { get; set; }
    }
    public class GetReplyResponseDTO 
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CommentId { get; set; }
        public UserDTO User { get; set; }
        public int ReplyCount { get; set; }
    }
}
