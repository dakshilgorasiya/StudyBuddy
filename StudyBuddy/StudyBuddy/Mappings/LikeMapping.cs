using AutoMapper;
using StudyBuddy.Models;
using StudyBuddy.DTOs;

namespace StudyBuddy.Mappings
{
    public class LikeMapping : Profile
    {
        public LikeMapping() 
        {
            CreateMap<LikePostRequestDTO, Like>();
            CreateMap<LikeCommentRequestDTO, Like>();
        }
    }
}
