using AutoMapper;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.SubjectBooks.Commands;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Subjects.Responses;
using PhyGen.Application.Topics.Commands;
using PhyGen.Application.Topics.Responses;
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
            // Mapping for Auth
            
            // Mapping for Curriculum
            CreateMap<Curriculum, CurriculumResponse>();
            CreateMap<CreateCurriculumCommand, Curriculum>();
            CreateMap<UpdateCurriculumCommand, Curriculum>();

            // Mapping for Subject
            CreateMap<Subject, SubjectResponse>();
            CreateMap<CreateSubjectCommand, Subject>();
            CreateMap<UpdateSubjectCommand, Subject>();

            // Mapping for SubjectBook
            CreateMap<SubjectBook, SubjectBookResponse>();
            CreateMap<CreateSubjectBookCommand, SubjectBook>();
            CreateMap<UpdateSubjectBookCommand, SubjectBook>();

            // Mapping for Chapter
            CreateMap<Chapter, ChapterResponse>();
            CreateMap<CreateChapterCommand, Chapter>();
            CreateMap<UpdateChapterCommand, Chapter>();

            // Mapping for Topic
            CreateMap<Topic, TopicResponse>();
            CreateMap<CreateTopicCommand, Topic>();
            CreateMap<UpdateTopicCommand, Topic>();
        }
    }
}
