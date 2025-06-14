using AutoMapper;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Response;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.ContentItemExamCategories.Commands;
using PhyGen.Application.ContentItemExamCategories.Responses;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.ContentItems.Responses;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategories.Responses;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Application.ExamCategoryChapters.Responses;
using PhyGen.Application.ExamQuestions.Commands;
using PhyGen.Application.ExamQuestions.Responses;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Application.MatrixContentItems.Commands;
using PhyGen.Application.MatrixContentItems.Responses;
using PhyGen.Application.QuestionMedias.Commands;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.SubjectCurriculums.Commands;
using PhyGen.Application.SubjectCurriculums.Responses;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Subjects.Responses;
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
            CreateMap<RegisterRequest, RegisterDto>();
            CreateMap<LoginRequest, LoginDto>();
            // Mapping for Curriculum
            CreateMap<Curriculum, CurriculumResponse>();
            CreateMap<CreateCurriculumCommand, Curriculum>();
            CreateMap<UpdateCurriculumCommand, Curriculum>();

            // Mapping for Chapter
            CreateMap<Chapter, ChapterResponse>();
            CreateMap<CreateChapterCommand, Chapter>();
            CreateMap<UpdateChapterCommand, Chapter>();

            // Mapping for SubjectCurriculum
            CreateMap<SubjectCurriculum, SubjectCurriculumResponse>();
            CreateMap<CreateSubjectCurriculumCommand, SubjectCurriculum>();
            CreateMap<UpdateSubjectCurriculumCommand, SubjectCurriculum>();

            // Mapping for ChapterUnit
            CreateMap<ChapterUnit, ChapterUnitResponse>();
            CreateMap<CreateChapterUnitCommand, ChapterUnit>();
            CreateMap<UpdateChapterUnitCommand, ChapterUnit>();

            //Mapping for Subject
            CreateMap<Subject, SubjectResponse>();
            CreateMap<CreateSubjectCommand, Subject>();
            CreateMap<UpdateSubjectCommand, Subject>();

            //Mapping for Exam
            CreateMap<Exam, ExamResponse>();
            CreateMap<CreateExamCommand, Exam>();
            CreateMap<UpdateExamCommand, Exam>();
            CreateMap<DeleteExamCommand, Exam>();

            // Mapping for ExamCategoryChapter
            CreateMap<ExamCategoryChapter, ExamCategoryChapterResponse>();
            CreateMap<CreateExamCategoryChapterCommand, ExamCategoryChapter>();
            CreateMap<UpdateExamCategoryChapterCommand, ExamCategoryChapter>();

            // Mapping for Content Flow
            CreateMap<ContentFlow, ContentFlowResponse>();
            CreateMap<CreateContentFlowCommand, ContentFlow>();
            CreateMap<UpdateContentFlowCommand, ContentFlow>();

            // Mapping for Content Item
            CreateMap<ContentItem, ContentItemResponse>();
            CreateMap<CreateContentItemCommand, ContentItem>();
            CreateMap<UpdateContentItemCommand, ContentItem>();

            // Mapping for Exam Category
            CreateMap<ExamCategory, ExamCategoryResponse>();
            CreateMap<CreateExamCategoryCommand, ExamCategory>();
            CreateMap<UpdateExamCategoryCommand, ExamCategory>();

            // Mapping for Content Item Exam Category
            CreateMap<ContentItemExamCategory, ContentItemExamCategoryResponse>();
            CreateMap<CreateContentItemExamCategoryCommand, ContentItemExamCategory>();
            CreateMap<UpdateContentItemExamCategoryCommand, ContentItemExamCategory>();

            // Mapping for Question
            CreateMap<Question, QuestionResponse>();
            CreateMap<CreateQuestionCommand, Question>();
            CreateMap<UpdateQuestionCommand, Question>();
            CreateMap<DeleteQuestionCommand, Question>();

            // Mapping for Exam Question
            CreateMap<ExamQuestion, ExamQuestionResponse>();
            CreateMap<CreateExamQuestionCommand, ExamQuestion>();
            CreateMap<UpdateExamQuestionCommand, ExamQuestion>();

            // Mapping for Question Media
            CreateMap<QuestionMedia, QuestionMediaResponse>();
            CreateMap<CreateQuestionMediaCommand, QuestionMedia>();
            CreateMap<UpdateQuestionMediaCommand, QuestionMedia>();

            // Mapping for Matrix
            CreateMap<Matrix, MatrixResponse>();
            CreateMap<CreateMatrixCommand, Matrix>();
            CreateMap<UpdateMatrixCommand, Matrix>();
            CreateMap<DeleteMatrixCommand, Matrix>();

            // Mapping for Matrix Content Item
            CreateMap<MatrixContentItem, MatrixContentItemResponse>();
            CreateMap<CreateMatrixContentItemCommand, MatrixContentItem>();
            CreateMap<UpdateMatrixContentItemCommand, MatrixContentItem>();
        }
    }
}
