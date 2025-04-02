using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface IFollowService
    {
        Task<string> FollowUserAsync(FollowUserRequestDTO followDTO);
        Task<CheckFollowingResponseDTO> CheckFollowingAsync(int id);
        Task<GetFollowersResponseDTO> GetFollowersAsync(int id);
        Task<GetFollowingResponseDTO> GetFollowingAsync(int id);
    }
}
