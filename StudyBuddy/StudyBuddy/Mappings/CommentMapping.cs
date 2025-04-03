using AutoMapper;
using StudyBuddy.DTOs;
using StudyBuddy.Models;

namespace StudyBuddy.Mappings
{
    public class CommentMapping : Profile
    {
        public CommentMapping()
        {
            CreateMap<PostCommentRequestDTO, Comment>()
                .ForMember(dest => dest.Replies, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore());

            CreateMap<Comment, PostCommentResponseDTO>();

            CreateMap<PostReplyRequestDTO, Comment>();

            CreateMap<Comment, PostReplyResponseDTO>();

            CreateMap<Comment, GetCommentsResponseDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }))
                .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies.Count));

            CreateMap<Comment, GetReplyResponseDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }))
                .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies.Count));

            CreateMap<User, UserDTO>();
        }
    }
}
