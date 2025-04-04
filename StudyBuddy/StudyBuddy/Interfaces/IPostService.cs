using StudyBuddy.DTOs;
using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IPostService
    {
        Task<CreatePostResponceDTO> CreatePostAsync(CreatePostRequestDTO createPostDto);
        Task<GetAllPostsResponseDTO> GetAllPostsAsync(int page, int pagesize);
        Task<GetPostByIdResponseDTO> GetPostByIdAsync(int postId);
    }
}
