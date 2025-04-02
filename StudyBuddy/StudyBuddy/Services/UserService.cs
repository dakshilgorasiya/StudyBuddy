using AutoMapper;
using StudyBuddy.Common;
using StudyBuddy.DTOs;
using StudyBuddy.Helpers;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using System.Security.Claims;

namespace StudyBuddy.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IMapper mapper, JwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<UserGetCurrentResponseDTO> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            var user = await _userRepository.GetUserById(int.Parse(userId));

            if(user == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "Unauthorized");
            }

            return new UserGetCurrentResponseDTO
            {
                Username = user.Username,
                Email = user.Email,
                Bio = user.Bio,
            };
        }

    }

}
