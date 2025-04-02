using StudyBuddy.DTOs;

namespace StudyBuddy.Interfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponseDTO> RegisterUser(UserRegisterRequestDTO dto);
        Task<UserLoginResponseDTO> LoginUser(UserLoginRequestDTO dto);
        Task<UserGetCurrentResponseDTO> GetCurrentUser();
    }
}
