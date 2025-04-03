using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
