using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email);
        Task<User> Register(User user);
        Task<User> GetUserByEmail(string email);
    }
}
