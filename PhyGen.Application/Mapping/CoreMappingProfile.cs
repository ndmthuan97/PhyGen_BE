using AutoMapper;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.ContentItems.Responses;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategories.Responses;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Application.MatrixSectionDetails.Responses;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Application.Notification.Responses;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.QuestionSections.Responses;
using PhyGen.Application.Sections.Responses;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Subjects.Responses;
using PhyGen.Application.Topics.Commands;
using PhyGen.Application.Topics.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;


namespace PhyGen.Application.Mapping
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            // Add your mapping configurations here
            // Mapping for Auth
            CreateMap<RegisterRequest, RegisterDto>();
            CreateMap<LoginRequest, LoginDto>();

            //Mapping for Notification
            CreateMap<Domain.Entities.Notification, NotificationResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Mapping for Curriculum
            CreateMap<Curriculum, CurriculumResponse>();
            CreateMap<Pagination<Curriculum>, Pagination<CurriculumResponse>>();

            // Mapping for Subject
            CreateMap<Subject, SubjectResponse>();

            // Mapping for SubjectBook
            CreateMap<SubjectBook, SubjectBookResponse>();
            CreateMap<Pagination<SubjectBook>, Pagination<SubjectBookResponse>>();

            // Mapping for Chapter
            CreateMap<Chapter, ChapterResponse>();
            CreateMap<Pagination<Chapter>, Pagination<ChapterResponse>>();

            // Mapping for Topic
            CreateMap<Topic, TopicResponse>();
            CreateMap<Pagination<Topic>, Pagination<TopicResponse>>();
            //CreateMap<List<Topic>, List<TopicResponse>>();

            // Mapping for ContentFlow
            CreateMap<ContentFlow, ContentFlowResponse>();

            // Mapping for ContentItem
            CreateMap<ContentItem, ContentItemResponse>();

            // Mapping for ExamCategory
            CreateMap<ExamCategory, ExamCategoryResponse>();

            // Mapping for Question
            CreateMap<Question, QuestionResponse>();
            CreateMap<Pagination<Question>, Pagination<QuestionResponse>>();

            // Mapping for Matrix
            CreateMap<Matrix, MatrixResponse>();
            CreateMap<Pagination<Matrix>, Pagination<MatrixResponse>>();

            // Mapping for MatrixSection
            CreateMap<MatrixSection, MatrixSectionResponse>();
            CreateMap<Pagination<MatrixSection>, Pagination<MatrixSectionResponse>>();

            // Mapping for MatrixSectionDetail
            CreateMap<MatrixSectionDetail, MatrixSectionDetailResponse>();

            // Mapping for Question Media
            CreateMap<QuestionMedia, QuestionMediaResponse>();

            // Mapping for Section
            CreateMap<Section, SectionResponse>();

            // Mapping for QuestionSection
            CreateMap<QuestionSection, QuestionSectionResponse>();

            // Mapping for Exam
            CreateMap<Exam, ExamResponse>();
            CreateMap<Pagination<Exam>, Pagination<ExamResponse>>();
        }
    }
}
