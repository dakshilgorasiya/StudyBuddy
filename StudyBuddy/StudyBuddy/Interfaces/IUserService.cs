using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUser(UserRegisterRequestDTO dto);
    }
}
