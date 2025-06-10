using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.SubjectCurriculums.Commands;
using PhyGen.Application.Subjects.Commands;

namespace PhyGen.API.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // Add your mapping configurations here
            CreateMap<RegisterDto, RegisterRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword.Trim()))
                .ReverseMap();

            CreateMap<LoginDto, LoginRequest>().ReverseMap();
            CreateMap<User, UserDtos>();
            // Mapping for Curriculum
            CreateMap<CreateCurriculumRequest, CreateCurriculumCommand>();
            CreateMap<UpdateCurriculumRequest, UpdateCurriculumCommand>();

            // Mapping for Chapter
            CreateMap<CreateChapterRequest, CreateChapterCommand>();
            CreateMap<UpdateChapterRequest, UpdateChapterCommand>();

            // Mapping for ChapterUnit
            CreateMap<CreateChapterUnitRequest, CreateChapterUnitCommand>();
            CreateMap<UpdateChapterUnitRequest, UpdateChapterUnitCommand>();

            // Mapping for SubjectCurriculum
            CreateMap<CreateSubjectCurriculumRequest, CreateSubjectCurriculumCommand>();
            CreateMap<UpdateSubjectCurriculumRequest, UpdateSubjectCurriculumCommand>();

            // Mapping for Subject
            CreateMap<CreateSubjectRequest, CreateSubjectCommand>();
            CreateMap<UpdateSubjectRequest, UpdateSubjectCommand>();

            // Mapping for Exam
            CreateMap<CreateExamRequest, CreateExamCommand>();
            CreateMap<UpdateExamRequest, UpdateExamCommand>();
            CreateMap<DeleteExamRequest, DeleteExamCommand>();
        }
    }
}
