using StudyBuddy.DTOs;
using StudyBuddy.Models;

namespace StudyBuddy.Interfaces
{
    public interface IPostService
    {
        Task<CreatePostResponceDTO> CreatePostAsync(CreatePostRequestDTO createPostDto);
    }
}
