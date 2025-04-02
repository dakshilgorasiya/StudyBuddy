using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class UserRegisterRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [MaxLength(20, ErrorMessage = "Username must be at most 20 characters long")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(50, ErrorMessage = "Password must be at most 50 characters long")]
        public string Password { get; set; }

        public string? Bio { get; set; }
    }

    public class UserRegisterResponseDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
    }

    public class UserLoginRequestDTO 
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(50, ErrorMessage = "Password must be at most 50 characters long")]
        public string Password { get; set; }
    }

    public class UserLoginResponseDTO 
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
    }
}
