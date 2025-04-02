using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IFollowRepository
    {
        Task<Follow?> ToggleFollowAsync(Follow follow);
        Task<Follow?> GetFollowAsync(int followedById, int followedToId);
        Task<int> GetFollowersAsync(int id);
        Task<int> GetFollowingAsync(int id);
    }
}
