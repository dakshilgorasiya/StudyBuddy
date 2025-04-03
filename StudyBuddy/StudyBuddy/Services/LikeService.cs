using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using System.Security.Claims;
using StudyBuddy.Common;
using StudyBuddy.Models;

namespace StudyBuddy.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepository, IPostRepository postRepository, ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<string> LikePostAsync(LikePostRequestDTO likedto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }
            bool isPostExist = await _postRepository.IsPostExists(likedto.PostId);
            if (!isPostExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }
            var like = _mapper.Map<Like>(likedto);
            like.OwnerId = int.Parse(userId);
            var result = await _likeRepository.ToggleLikeAsync(like);
            if(result == null)
            {
                return "Unliked";
            }
            else
            {
                return "Liked";
            }
        }

        public async Task<string> LikeCommentAsync(LikeCommentRequestDTO commentdto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }
            bool isCommentExist = await _commentRepository.IsCommentExists(commentdto.CommentId);
            if (!isCommentExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }
            var like = _mapper.Map<Like>(commentdto);
            like.OwnerId = int.Parse(userId);
            var result = await _likeRepository.ToggleLikeAsync(like);
            if (result == null)
            {
                return "Unliked";
            }
            else
            {
                return "Liked";
            }
        }

        public async Task<GetPostLikesResponseDTO> GetPostLikesAsync(int postId)
        {
            bool isPostExist = await _postRepository.IsPostExists(postId);
            if (!isPostExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }
            int likes = await _likeRepository.CountPostLikeAsync(postId);
            var response = new GetPostLikesResponseDTO
            {
                LikesCount = likes
            };
            return response;
        }

        public async Task<GetCommentLikesResponseDTO> GetCommentLikesAsync(int commentId)
        {
            bool isCommentExist = await _commentRepository.IsCommentExists(commentId);
            if (!isCommentExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }
            int likes = await _likeRepository.CountCommentLikeAsync(commentId);
            var response = new GetCommentLikesResponseDTO
            {
                LikesCount = likes
            };
            return response;
        }

        public async Task<CheckPostLikeResponseDTO> CheckPostLikeAsync(int postId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }
            bool isPostExist = await _postRepository.IsPostExists(postId);
            if (!isPostExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }
            bool isLiked = await _likeRepository.CheckPostLikeAsync(postId, int.Parse(userId));
            var response = new CheckPostLikeResponseDTO
            {
                IsLiked = isLiked
            };
            return response;
        }

        public async Task<CheckCommentLikeResponseDTO> CheckCommentLikeAsync(int commentId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }
            bool isCommentExist = await _commentRepository.IsCommentExists(commentId);
            if (!isCommentExist)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }
            bool isLiked = await _likeRepository.CheckCommentLikeAsync(commentId, int.Parse(userId));
            var response = new CheckCommentLikeResponseDTO
            {
                IsLiked = isLiked
            };
            return response;
        }
    }
}
