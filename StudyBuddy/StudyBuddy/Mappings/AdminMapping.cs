using AutoMapper;
using StudyBuddy.Models;
using StudyBuddy.DTOs;

namespace StudyBuddy.Mappings
{
    public class AdminMapping : Profile
    {
        public AdminMapping() 
        {
            CreateMap<Report, GetAllReportsReponseDTO>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }));

            CreateMap<Report, GetAllReportsOfPostResponseDTO>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }));

            CreateMap<Report, MarkReportAsSolvedResponseDTO>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new UserDTO
                {
                    Id = src.Owner.Id,
                    Username = src.Owner.Username,
                    Email = src.Owner.Email,
                    Avatar = src.Owner.Avatar
                }));
        }
    }
}
