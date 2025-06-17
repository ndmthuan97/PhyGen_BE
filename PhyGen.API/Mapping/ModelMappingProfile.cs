using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentItemExamCategories.Commands;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Application.ExamQuestions.Commands;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.MatrixContentItems.Commands;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Responses;
using PhyGen.Application.PayOs.Response;
using PhyGen.Application.QuestionMedias.Commands;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.SubjectCurriculums.Commands;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Domain.Entities;

namespace PhyGen.API.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // Add your mapping configurations here
            CreateMap<RegisterDto, RegisterRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Trim()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword.Trim()))
                .ReverseMap();
            CreateMap<LoginDto, LoginRequest>().ReverseMap();

            //Mapping fot User
            CreateMap<User, UserDtos>();

            //Mapping for Payments
            CreateMap<Payments, SearchPaymentResponse>();

            // Mapping for Notification
            CreateMap<Notification, NotificationResponse>();

            CreateMap<UpdateNotificationRequest, UpdateNotificationCommand>();
            CreateMap<CreateNotificationRequest, CreateNotificationCommand>();

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

            // Mapping for ExamCategoryChapter
            CreateMap<CreateExamCategoryChapterRequest, CreateExamCategoryChapterCommand>();
            CreateMap<UpdateExamCategoryChapterRequest, UpdateExamCategoryChapterCommand>();

            //Mapping for Content Flow
            CreateMap<CreateContentFlowRequest, CreateContentFlowCommand>();
            CreateMap<UpdateContentFlowRequest, UpdateContentFlowCommand>();

            // Mapping for Content Item
            CreateMap<CreateContentItemRequest, CreateContentItemCommand>();
            CreateMap<UpdateContentItemRequest, UpdateContentItemCommand>();

            // Mapping for Exam Category
            CreateMap<CreateExamCategoryRequest, CreateExamCategoryCommand>();
            CreateMap<UpdateExamCategoryRequest, UpdateExamCategoryCommand>();

            // Mapping for Content Item Exam Category
            CreateMap<CreateContentItemExamCategoryRequest, CreateContentItemExamCategoryCommand>();
            CreateMap<UpdateContentItemExamCategoryRequest, UpdateContentItemExamCategoryCommand>();

            // Mapping for Question
            CreateMap<CreateQuestionRequest, CreateQuestionCommand>();
            CreateMap<UpdateQuestionRequest, UpdateQuestionCommand>();

            // Mapping for Exam Question
            CreateMap<CreateExamQuestionRequest, CreateExamQuestionCommand>();
            CreateMap<UpdateExamQuestionRequest, UpdateExamQuestionCommand>();

            // Mapping for Question Media
            CreateMap<CreateQuestionMediaRequest, CreateQuestionMediaCommand>();
            CreateMap<UpdateQuestionMediaRequest, UpdateQuestionMediaCommand>();

            // Mapping for Matrix
            CreateMap<CreateMatrixRequest, CreateMatrixCommand>();
            CreateMap<UpdateMatrixRequest, UpdateMatrixCommand>();
            CreateMap<DeleteMatrixRequest, DeleteMatrixCommand>();

            // Mapping for Matrix Content Item
            CreateMap<CreateMatrixContentItemRequest, CreateMatrixContentItemCommand>();
            CreateMap<UpdateMatrixContentItemRequest, UpdateMatrixContentItemCommand>();
        }
    }
}
