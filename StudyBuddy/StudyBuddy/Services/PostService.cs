using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Helpers;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using StudyBuddy.Common;
using System.Security.Claims;

namespace StudyBuddy.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IPostRepository postRepository, CloudinaryHelper cloudinaryHelper, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _cloudinaryHelper = cloudinaryHelper;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreatePostResponceDTO> CreatePostAsync(CreatePostRequestDTO createPostDto)

        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            var imageUrls = createPostDto.Images != null && createPostDto.Images.Count > 0
                ? await _cloudinaryHelper.UploadImagesAsync(createPostDto.Images)
                : new List<string>();

            var newPost = _mapper.Map<Post>(createPostDto);
            newPost.Images = imageUrls.ToArray();
            newPost.OwnerId = int.Parse(userId);

            var createdPost = await _postRepository.CreatePostAsync(newPost);

            return _mapper.Map<CreatePostResponceDTO>(createdPost);
        }
    }
}
