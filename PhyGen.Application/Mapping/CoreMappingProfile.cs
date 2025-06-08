using AutoMapper;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Response;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Response;
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
        }
    }
}
