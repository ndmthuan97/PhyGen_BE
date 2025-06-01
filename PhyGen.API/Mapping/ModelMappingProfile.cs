using AutoMapper;
using PhyGen.API.Models.Answers;
using PhyGen.API.Models.Curriculums;
using PhyGen.API.Models.Questions;
using PhyGen.Application.Answers.Commands;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Chapters.Commands;
using PhyGen.API.Models.Chapters;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Systems.Users;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.API.Models.ChapterUnits;
using PhyGen.API.Models.Exam;
using PhyGen.Application.Exams.Commands;
using PhyGen.Domain.Entities;

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
            CreateMap<User, UserDtos>();
            // Add your mapping configurations here

            // Mapping for Curriculum
            CreateMap<CreateCurriculumRequest, CreateCurriculumCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateCurriculumRequest, UpdateCurriculumCommand>();

            // Mapping for Chapter
            CreateMap<CreateChapterRequest, CreateChapterCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateChapterRequest, UpdateChapterCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            CreateMap<DeleteChapterRequest, DeleteChapterCommand>()
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy));

            // Mapping for Chapter Unit
            CreateMap<CreateChapterUnitRequest, CreateChapterUnitCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateChapterUnitRequest, UpdateChapterUnitCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));

            // Mapping for Question
            CreateMap<CreateQuestionRequest, CreateQuestionCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateQuestionRequest, UpdateQuestionCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));

            // Mapping for Answer
            CreateMap<CreateAnswerRequest, CreateAnswerCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateAnswerRequest, UpdateAnswerCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));

            // Mapping for Exam
            CreateMap<CreateExamRequest, CreateExamCommand>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateExamRequest, UpdateExamCommand>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
        }
    }
}
