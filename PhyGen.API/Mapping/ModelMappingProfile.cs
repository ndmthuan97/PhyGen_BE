using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.Curriculums.Commands;

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

            // Mapping for Chapter
            CreateMap<CreateChapterRequest, CreateChapterCommand>();
            CreateMap<UpdateChapterRequest, UpdateChapterCommand>();

            // Mapping for ChapterUnit
            CreateMap<CreateChapterUnitRequest, CreateChapterUnitCommand>();
            CreateMap<UpdateChapterUnitRequest, UpdateChapterUnitCommand>();
        }
    }
}
