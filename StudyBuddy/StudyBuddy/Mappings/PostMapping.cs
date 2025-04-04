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

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }));

            CreateMap<Post, GetPostByIdResponseDTO>()
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.OwnerFollowers, opt => opt.MapFrom(src => src.Owner.Followers.Count));
        }
    }
}
