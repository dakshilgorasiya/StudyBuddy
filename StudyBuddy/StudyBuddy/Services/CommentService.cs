using StudyBuddy.DTOs;
using StudyBuddy.Interfaces;
using AutoMapper;
using System.Security.Claims;
using StudyBuddy.Common;
using StudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _postRepository = postRepository;
            _userRepository = userRepository;
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
            bool postExists = await _postRepository.IsPostExists(postId);
            if (!postExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Post not found");
            }

            var comments = await _commentRepository.GetAllCommentsByPostId(postId);

            return _mapper.Map<List<GetCommentsResponseDTO>>(comments);
        }

        public async Task<List<GetReplyResponseDTO>> GetRepliesAsync(int commentId)
        {
            bool commentExists = await _commentRepository.IsCommentExists(commentId);
            if (!commentExists)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Comment not found");
            }

            var replies = await _commentRepository.GetAllRepliesByCommentId(commentId);
            return _mapper.Map<List<GetReplyResponseDTO>>(replies);
        }
    }
}
