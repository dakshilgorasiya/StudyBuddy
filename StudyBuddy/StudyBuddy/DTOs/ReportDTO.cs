using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class CreateReportRequestDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage = "Title must be at most 100 characters long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(3, ErrorMessage = "Content must be at least 3 characters long")]
        public string Content { get; set; }

        [Required(ErrorMessage = "PostId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PostId must be a positive integer")]
        public int PostId { get; set; }
    }

    public class CreateReportResponseDTO
    { 
        public string Title { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public bool IsSolved { get; set; }
    }
}
