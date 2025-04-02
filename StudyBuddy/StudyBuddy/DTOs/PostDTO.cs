using System.ComponentModel.DataAnnotations;
using StudyBuddy.Validation;

namespace StudyBuddy.DTOs
{
    public class CreatePostRequestDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Title must be at most 50 characters long")]
        public string Title { get; set; }

        public string[] Tags { get; set; }

        [Required(ErrorMessage = "Tweet data is required")]
        [MinLength(3, ErrorMessage = "Tweet data must be at least 3 characters long")]
        public string TweetData { get; set; }

        [ImageOnly(ErrorMessage = "Only images are allowed")]
        public List<IFormFile> Images { get; set; }
    }

    public class CreatePostResponceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string[] Tags { get; set; }
        public string TweetData { get; set; }
        public string[] Images { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
