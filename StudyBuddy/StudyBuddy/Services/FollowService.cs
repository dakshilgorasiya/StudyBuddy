using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using StudyBuddy.Common;
using System.Security.Claims;

namespace StudyBuddy.Services
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FollowService(IFollowRepository followRepository, IMapper mapper, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _followRepository = followRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> FollowUserAsync(FollowUserRequestDTO followDTO)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            Follow follow = new Follow();

            follow.FollowedById = int.Parse(userId);
            follow.FollowedToId = followDTO.Id;

            if(follow.FollowedById == follow.FollowedToId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "You can't follow yourself");
            }

            User userExits = await _userRepository.GetUserById(follow.FollowedToId);

            if(userExits == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User not found");
            }

            var result = await _followRepository.ToggleFollowAsync(follow);

            if(result == null)
            {
                return "Unfollowed";
            }
            else
            {
                return "Followed";
            }
        }
        public async Task<CheckFollowingResponseDTO> CheckFollowingAsync(int id) 
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            if(int.Parse(userId) == id)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Invalid Request");
            }

            User userExits = await _userRepository.GetUserById(id);

            if (userExits == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User not found");
            }

            var result = await _followRepository.GetFollowAsync(int.Parse(userId), id);

            CheckFollowingResponseDTO checkFollowingResponseDTO = new CheckFollowingResponseDTO();
            checkFollowingResponseDTO.IsFollowing = result != null;

            return checkFollowingResponseDTO;
        }
        public async Task<GetFollowersResponseDTO> GetFollowersAsync(int id) 
        {
            User userExits = await _userRepository.GetUserById(id);

            if (userExits == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User not found");
            }

            var result = await _followRepository.GetFollowersAsync(id);

            GetFollowersResponseDTO getFollowersResponseDTO = new GetFollowersResponseDTO();

            getFollowersResponseDTO.followersCount = result;

            return getFollowersResponseDTO;
        }
        public async Task<GetFollowingResponseDTO> GetFollowingAsync(int id) 
        {
            User userExits = await _userRepository.GetUserById(id);

            if (userExits == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User not found");
            }
            
            var result = await _followRepository.GetFollowingAsync(id);

            GetFollowingResponseDTO getFollowingResponseDTO = new GetFollowingResponseDTO();

            getFollowingResponseDTO.followingCount = result;

            return getFollowingResponseDTO;
        }
    }
}
