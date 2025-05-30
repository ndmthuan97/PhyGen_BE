using AutoMapper;
using PhyGen.API.Models.Curriculums;
using PhyGen.API.Models.Questions;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Systems.Users;

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


            // Mapping for Question
            CreateMap<CreateQuestionRequest, CreateQuestionCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateQuestionRequest, UpdateQuestionCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));



        }
    }
}
