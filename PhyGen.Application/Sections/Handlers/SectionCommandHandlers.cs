using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.Sections.Commands;
using PhyGen.Application.Sections.Exceptions;
using PhyGen.Application.Sections.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Handlers
{
    public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, SectionResponse>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IExamRepository _examRepository;

        public CreateSectionCommandHandler(ISectionRepository sectionRepository, IExamRepository examRepository)
        {
            _sectionRepository = sectionRepository;
            _examRepository = examRepository;
        }

        public async Task<SectionResponse> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            if (await _examRepository.GetByIdAsync(request.ExamId) == null)
            {
                throw new ExamNotFoundException();
            }

            var section = new Section
            {
                ExamId = request.ExamId,
                Title = request.Title,
                Description = request.Description,
                SectionType = request.SectionType,
                DisplayOrder = request.DisplayOrder
            };

            await _sectionRepository.AddAsync(section);
            return AppMapper<CoreMappingProfile>.Mapper.Map<SectionResponse>(section);
        }
    }

    public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, Unit>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IExamRepository _examRepository;

        public UpdateSectionCommandHandler(ISectionRepository sectionRepository, IExamRepository examRepository)
        {
            _sectionRepository = sectionRepository;
            _examRepository = examRepository;
        }

        public async Task<Unit> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
        {
            if (await _examRepository.GetByIdAsync(request.ExamId) == null)
            {
                throw new ExamNotFoundException();
            }

            var section = await _sectionRepository.GetByIdAsync(request.Id);
            if (section == null)
            {
                throw new SectionNotFoundException();
            }

            section.Title = request.Title;
            section.Description = request.Description;
            section.SectionType = request.SectionType;
            section.DisplayOrder = request.DisplayOrder;

            await _sectionRepository.UpdateAsync(section);
            return Unit.Value;
        }
    }

    public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, Unit>
    {
        private readonly ISectionRepository _sectionRepository;

        public DeleteSectionCommandHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Unit> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            var section = await _sectionRepository.GetByIdAsync(request.Id);
            if (section == null || section.DeletedAt.HasValue)
            {
                throw new SectionNotFoundException();
            }
            
            await _sectionRepository.UpdateAsync(section);
            return Unit.Value;
        }
    }
}
