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

            CreateMap<Post, GetAllPostsResponseDTO>()
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Count));

            CreateMap<Post, GetPostByIdResponseDTO>()
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.OwnerFollowers, opt => opt.MapFrom(src => src.Owner.Followers.Count));
        }
    }
}
