using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using AutoMapper;
using System.Security.Claims;
using StudyBuddy.Common;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace StudyBuddy.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository, IUserRepository userRepository, IMemoryCache cache)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<PostCommentResponseDTO> PostCommentAsync(PostCommentRequestDTO commentDTO)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            bool postExists = await _postRepository.IsPostExists(commentDTO.PostId);
            if(!postExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }

            Comment comment = _mapper.Map<Comment>(commentDTO);
            comment.OwnerId = int.Parse(userId);
            var result = await _commentRepository.AddCommentAsync(comment);
            return _mapper.Map<PostCommentResponseDTO>(result);
        }

        public async Task<PostReplyResponseDTO> PostReplyAsync(PostReplyRequestDTO replyDTO)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorResponse(StatusCodes.Status401Unauthorized, "User is not authenticated");
            }

            bool commentExists = await _commentRepository.IsCommentExists(replyDTO.CommentId);
            if (!commentExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }

            Comment reply = _mapper.Map<Comment>(replyDTO);
            reply.OwnerId = int.Parse(userId);
            var result = await _commentRepository.AddCommentAsync(reply);
            return _mapper.Map<PostReplyResponseDTO>(result);
        }

        public async Task<List<GetCommentsResponseDTO>> GetCommentsAsync(int postId)
        {
            string cacheKey = "comments_" + postId;

            if(_cache.TryGetValue(cacheKey, out List<GetCommentsResponseDTO> cachedComments))
            {
                return cachedComments;
            }

            bool postExists = await _postRepository.IsPostExists(postId);
            if (!postExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }

            var comments = await _commentRepository.GetAllCommentsByPostId(postId);

            var response = _mapper.Map<List<GetCommentsResponseDTO>>(comments);

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }

        public async Task<List<GetReplyResponseDTO>> GetRepliesAsync(int commentId)
        {
            string cacheKey = "replies_" + commentId;

            if (_cache.TryGetValue(cacheKey, out List<GetReplyResponseDTO> cachedReplies))
            {
                return cachedReplies;
            }

            bool commentExists = await _commentRepository.IsCommentExists(commentId);
            if (!commentExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }

            var replies = await _commentRepository.GetAllRepliesByCommentId(commentId);
            var response = _mapper.Map<List<GetReplyResponseDTO>>(replies);

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }
    }
}
