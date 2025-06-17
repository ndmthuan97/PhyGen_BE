using AutoMapper;
using PhyGen.API.Models;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.SubjectBooks.Commands;
using PhyGen.Application.Subjects.Commands;
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

            // Mapping for Subject
            CreateMap<CreateSubjectRequest, CreateSubjectCommand>();
            CreateMap<UpdateSubjectRequest, UpdateSubjectCommand>();

            // Mapping for SubjectBook
            CreateMap<CreateSubjectBookRequest, CreateSubjectBookCommand>();
            CreateMap<UpdateSubjectBookRequest, UpdateSubjectBookCommand>();
        }
    }
}
