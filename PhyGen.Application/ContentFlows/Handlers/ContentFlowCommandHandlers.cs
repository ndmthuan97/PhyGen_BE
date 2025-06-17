using MediatR;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class CreateContentFlowCommandHandler : IRequestHandler<CreateContentFlowCommand, Guid>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;

        public CreateContentFlowCommandHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Guid> Handle(CreateContentFlowCommand request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();
            
            if (await _repository.GetContentFlowByCurriculumIdAndNameAsync(request.CurriculumId, request.Name) != null)
                throw new ContentFlowSameNameException();

            var contentFlow = new ContentFlow
            {
                CurriculumId = request.CurriculumId,
                SubjectId = request.SubjectId,
                Name = request.Name,
                Description = request.Description
            };

            await _repository.AddAsync(contentFlow);
            return contentFlow.Id;
        }
    }

    public class UpdateContentFlowCommandHandler : IRequestHandler<UpdateContentFlowCommand, Unit>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;
        public UpdateContentFlowCommandHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
        }
        public async Task<Unit> Handle(UpdateContentFlowCommand request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            if (await _repository.GetContentFlowByCurriculumIdAndNameAsync(request.CurriculumId, request.Name) != null)
                throw new ContentFlowSameNameException();

            var contentFlow = await _repository.GetByIdAsync(request.Id);
            if (contentFlow == null)
                throw new ContentFlowNotFoundException();

            contentFlow.CurriculumId = request.CurriculumId;
            contentFlow.SubjectId = request.SubjectId;
            contentFlow.Name = request.Name;
            contentFlow.Description = request.Description;

            await _repository.UpdateAsync(contentFlow);
            return Unit.Value;
        }
    }
}
