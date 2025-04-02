using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Models;

namespace StudyBuddy.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserRegisterRequestDTO, User>();
            CreateMap<User, UserRegisterResponseDTO>();
            CreateMap<UserLoginRequestDTO, User>();
            CreateMap<User, UserLoginResponseDTO>();
            CreateMap<User, UserGetCurrentResponseDTO>();
        }
    }
}
