using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.MatrixSectionDetails.Commands;
using PhyGen.Application.MatrixSections.Commands;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Responses;
using PhyGen.Application.PayOs.Response;
using PhyGen.Application.QuestionMedias.Commands;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.QuestionSections.Commands;
using PhyGen.Application.Sections.Commands;
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
            CreateMap<Payment, SearchPaymentResponse>();

            // Mapping for Notification
            CreateMap<Notification, NotificationResponse>();
            CreateMap<UpdateNotificationRequest, UpdateNotificationReadCommand>();
            CreateMap<UpdateNotificationRequest, UpdateNotificationCommand>();
            CreateMap<CreateNotificationRequest, CreateNotificationCommand>();

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
            CreateMap<DeleteContentFlowRequest, DeleteContentFlowCommand>();

            // Mapping for ContentItem
            CreateMap<CreateContentItemRequest, CreateContentItemCommand>();
            CreateMap<UpdateContentItemRequest, UpdateContentItemCommand>();
            CreateMap<DeleteContentItemRequest, DeleteContentItemCommand>();

            // Mapping for ExamCategory
            CreateMap<CreateExamCategoryRequest, CreateExamCategoryCommand>();
            CreateMap<UpdateExamCategoryRequest, UpdateExamCategoryCommand>();
            CreateMap<DeleteExamCategoryRequest, DeleteExamCategoryCommand>();

            // Mapping for Question
            CreateMap<CreateQuestionRequest, CreateQuestionCommand>();
            CreateMap<UpdateQuestionRequest, UpdateQuestionCommand>();
            CreateMap<DeleteQuestionRequest, DeleteQuestionCommand>();

            // Mapping for Matrix
            CreateMap<CreateMatrixRequest, CreateMatrixCommand>();
            CreateMap<UpdateMatrixRequest, UpdateMatrixCommand>();
            CreateMap<DeleteMatrixRequest, DeleteMatrixCommand>();

            // Mapping for MatrixSection
            CreateMap<CreateMatrixSectionRequest, CreateMatrixSectionCommand>();
            CreateMap<UpdateMatrixSectionRequest, UpdateMatrixSectionCommand>();
            CreateMap<DeleteMatrixSectionRequest, DeleteMatrixSectionCommand>();

            // Mapping for MatrixSectionDetail
            CreateMap<CreateMatrixSectionDetailRequest, CreateMatrixSectionDetailCommand>();
            CreateMap<UpdateMatrixSectionDetailRequest, UpdateMatrixSectionDetailCommand>();
            CreateMap<DeleteMatrixSectionDetailRequest, DeleteMatrixSectionDetailCommand>();

            // Mapping for QuestionMedia
            CreateMap<CreateQuestionMediaRequest, CreateQuestionMediaCommand>();
            CreateMap<UpdateQuestionMediaRequest, UpdateQuestionMediaCommand>();
            CreateMap<DeleteQuestionMediaRequest, DeleteQuestionMediaCommand>();

            // Mapping for Section
            CreateMap<CreateSectionRequest, CreateSectionCommand>();
            CreateMap<UpdateSectionRequest, UpdateSectionCommand>();
            CreateMap<DeleteSectionRequest, DeleteSectionCommand>();

            // Mapping for QuestionSection
            CreateMap<CreateQuestionSectionRequest, CreateQuestionSectionCommand>();
            CreateMap<UpdateQuestionSectionRequest, UpdateQuestionSectionCommand>();

            // Mapping for Exam
            CreateMap<CreateExamRequest, CreateExamCommand>();
            CreateMap<UpdateExamRequest, UpdateExamCommand>();
            CreateMap<DeleteExamRequest, DeleteExamCommand>();
        }
    }
}
