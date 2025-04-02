using AutoMapper;
using StudyBuddy.Common;
using StudyBuddy.DTOs;
using StudyBuddy.Helpers;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public UserService(IUserRepository userRepository, IMapper mapper, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        public async Task<UserRegisterResponseDTO> RegisterUser(UserRegisterRequestDTO dto)
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

            var userResponse = _mapper.Map<UserRegisterResponseDTO>(user);

            return userResponse;
        }

        public async Task<UserLoginResponseDTO> LoginUser(UserLoginRequestDTO dto)
        {
            var user = await _userRepository.GetUserByEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return null; // Invalid credentials
            }

            string token = _jwtHelper.GenerateToken(user);
            return new UserLoginResponseDTO
            {
                Username = user.Username,
                Email = user.Email,
                Bio = user.Bio,
                Token = token
            };
        }

    }

}
