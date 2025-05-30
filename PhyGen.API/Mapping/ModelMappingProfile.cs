using AutoMapper;
using PhyGen.Application.Systems.Users;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.API.Models.Curriculums;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Chapters.Commands;
using PhyGen.API.Models.Chapters;

namespace PhyGen.API.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<RegisterDto, RegisterRequest>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim()))
                .ReverseMap();

            CreateMap<LoginDto, LoginRequest>().ReverseMap();
            // Add your mapping configurations here

            // Mapping for Curriculum
            CreateMap<CreateCurriculumRequest, CreateCurriculumCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateCurriculumRequest, UpdateCurriculumCommand>();

            // Mapping for Chapter
            CreateMap<CreateChapterRequest, CreateChapterCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateChapterRequest, UpdateChapterCommand>();
        }
    }
}
