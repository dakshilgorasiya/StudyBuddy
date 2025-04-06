using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Helpers;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using StudyBuddy.Common;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace StudyBuddy.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public PostService(IPostRepository postRepository, CloudinaryHelper cloudinaryHelper, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _postRepository = postRepository;
            _cloudinaryHelper = cloudinaryHelper;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
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

        public async Task<GetAllPostsResponseDTO> GetAllPostsAsync(int page, int pagesize)
        {
            string cacheKey = "posts_" + page + "_" + pagesize;

            if (_cache.TryGetValue(cacheKey, out GetAllPostsResponseDTO cachedPosts))
            {
                return cachedPosts;
            }

            var posts = await _postRepository.GetAllPostsAsync(page, pagesize);
            if (posts == null || !posts.Any())
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "No posts found");
            }
            List<PostDTO> postDtos = _mapper.Map<List<PostDTO>>(posts);
            GetAllPostsResponseDTO response = new GetAllPostsResponseDTO();
            response.Posts = postDtos;
            int postCount = await _postRepository.CountPost();
            response.TotalPages = (int)Math.Ceiling((double)postCount / pagesize);

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }

        public async Task<GetPostByIdResponseDTO> GetPostByIdAsync(int postId)
        {
            string cacheKey = $"post_{postId}";

            if (_cache.TryGetValue(cacheKey, out GetPostByIdResponseDTO cachedPost))
            {
                return cachedPost;
            }

            var post = await _postRepository.GetPostById(postId);
            if (post == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }
            GetPostByIdResponseDTO postDto = _mapper.Map<GetPostByIdResponseDTO>(post);

            _cache.Set(cacheKey, postDto, TimeSpan.FromMinutes(5));

            return postDto;
        }
    }
}
