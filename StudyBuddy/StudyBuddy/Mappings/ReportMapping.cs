using AutoMapper;
using StudyBuddy.Models;
using StudyBuddy.DTOs;

namespace StudyBuddy.Mappings
{
    public class ReportMapping : Profile
    {
        public ReportMapping()
        {
            CreateMap<CreateReportRequestDTO, Report>()
                .ForMember(dest => dest.IsSolved, opt => opt.MapFrom(src => false));

            CreateMap<Report, CreateReportResponseDTO> ();
        }
    }
}
