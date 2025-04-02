using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Models;

namespace StudyBuddy.Mappings
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<CreatePostRequestDTO, Post>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Post, CreatePostResponceDTO>();
        }
    }
}
