using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.DTOs
{
    public class FollowUserRequestDTO 
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
        public int Id { get; set; }
    }
    public class FollowUserResponseDTO 
    {

    }
    public class CheckFollowingResponseDTO
    {
        public bool IsFollowing { get; set; }
    }
    public class GetFollowersResponseDTO 
    {
        public int followersCount { get; set; }
    }
    public class GetFollowingResponseDTO 
    {
        public int followingCount { get; set; }
    }
}
