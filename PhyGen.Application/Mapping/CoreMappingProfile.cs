using AutoMapper;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Response;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Response;
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
        }
    }
}
