using AutoMapper;
using StudyBuddy.Common;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<string> RegisterUser(UserRegisterRequestDTO dto)
        {

            if (dto == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Invalid data provided.");
            }

            if (await _userRepository.UserExists(dto.Email))
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "User with this email already exists");
            }

            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password); // Hash password
            user.Role = UserRole.User;

            await _userRepository.Register(user);
            return "User registered successfully!";
        }
    }
}
