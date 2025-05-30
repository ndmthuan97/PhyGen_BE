using AutoMapper;
using PhyGen.API.Models.Curriculums;
using PhyGen.Application.Curriculums.Commands;

namespace PhyGen.API.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // Add your mapping configurations here

            // Mapping for Curriculum
            CreateMap<CreateCurriculumRequest, CreateCurriculumCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));

            CreateMap<UpdateCurriculumRequest, UpdateCurriculumCommand>();
        }
    }
}
