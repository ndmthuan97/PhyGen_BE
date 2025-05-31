using AutoMapper;

using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Answers.Commands;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Mapping
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            // Add your mapping configurations here

            // Mapping for Curriculum
            CreateMap<Curriculum, CurriculumResponse>();

            CreateMap<CreateCurriculumCommand, Curriculum>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateCurriculumCommand, Curriculum>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Mapping for Chapter
            CreateMap<Chapter, ChapterResponse>();

            CreateMap<CreateChapterCommand, Chapter>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateChapterCommand, Chapter>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            CreateMap<DeleteChapterCommand, Chapter>()
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy));

            // Mapping for Chapter Unit
            CreateMap<ChapterUnit, ChapterUnitResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Chapter.Title));

            CreateMap<CreateChapterUnitCommand, ChapterUnit>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateChapterUnitCommand, ChapterUnit>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));


            // Mapping for Question
            CreateMap<CreateQuestionCommand, Question>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateQuestionCommand, Question>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            //CreateMap<Question, QuestionResponse>()
            //    .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            // Mapping for Answer
            CreateMap<CreateAnswerCommand, Answer>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<UpdateAnswerCommand, Answer>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
        }
    }
}
