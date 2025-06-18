using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentItemExamCategories.Commands;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Application.SubjectBooks.Commands;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Topics.Commands;
using PhyGen.Domain.Entities;

namespace PhyGen.API.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // Add your mapping configurations here
            
            // Mapping for Curriculum
            CreateMap<CreateCurriculumRequest, CreateCurriculumCommand>();
            CreateMap<UpdateCurriculumRequest, UpdateCurriculumCommand>();
            CreateMap<DeleteCurriculumRequest, DeleteCurriculumCommand>();

            // Mapping for Subject
            CreateMap<CreateSubjectRequest, CreateSubjectCommand>();
            CreateMap<UpdateSubjectRequest, UpdateSubjectCommand>();

            // Mapping for SubjectBook
            CreateMap<CreateSubjectBookRequest, CreateSubjectBookCommand>();
            CreateMap<UpdateSubjectBookRequest, UpdateSubjectBookCommand>();
            CreateMap<DeleteSubjectBookRequest, DeleteSubjectBookCommand>();

            // Mapping for Chapter
            CreateMap<CreateChapterRequest, CreateChapterCommand>();
            CreateMap<UpdateChapterRequest, UpdateChapterCommand>();
            CreateMap<DeleteChapterRequest, DeleteChapterCommand>();

            // Mapping for Topic
            CreateMap<CreateTopicRequest, CreateTopicCommand>();
            CreateMap<UpdateTopicRequest, UpdateTopicCommand>();
            CreateMap<DeleteTopicRequest, DeleteTopicCommand>();

            // Mapping for ContentFlow
            CreateMap<CreateContentFlowRequest, CreateContentFlowCommand>();
            CreateMap<UpdateContentFlowRequest, UpdateContentFlowCommand>();

            // Mapping for ContentItem
            CreateMap<CreateContentItemRequest, CreateContentItemCommand>();
            CreateMap<UpdateContentItemRequest, UpdateContentItemCommand>();

            // Mapping for ExamCategory
            CreateMap<CreateExamCategoryRequest, CreateExamCategoryCommand>();
            CreateMap<UpdateExamCategoryRequest, UpdateExamCategoryCommand>();

            // Mapping for ContentItemExamCaterory
            CreateMap<CreateContentItemExamCategoryRequest, CreateContentItemExamCategoryCommand>();
            CreateMap<UpdateContentItemExamCategoryRequest, UpdateContentItemExamCategoryCommand>();

            // Mapping for ExamCategoryChapter
            CreateMap<CreateExamCategoryChapterRequest, CreateExamCategoryChapterCommand>();
            CreateMap<UpdateExamCategoryChapterRequest, UpdateExamCategoryChapterCommand>();
        }
    }
}
