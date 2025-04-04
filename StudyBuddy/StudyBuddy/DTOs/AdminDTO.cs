using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class GetAllReportsReponseDTO
    {
        public int Id { get; set; }
        public string Title {  get; set; }
        public string Content { get; set; }
        public bool IsSolved { get; set; }
        public UserDTO Owner { get; set; }
        public int PostId { get; set; }
    }

    public class GetAllReportsOfPostResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsSolved { get; set; }
        public UserDTO Owner { get; set; }
        public int PostId { get; set; }
    }

    public class MarkReportAsSolvedRequestDTO
    {
        [Required(ErrorMessage = "ReportId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ReportId must be a positive integer")]
        public int Id { get; set; }
    }

    public class MarkReportAsSolvedResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsSolved { get; set; }
        public UserDTO Owner { get; set; }
        public int PostId { get; set; }
    }
}
