using MediatR;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class CreateContentFlowCommandHandler : IRequestHandler<CreateContentFlowCommand, ContentFlowResponse>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;

        public CreateContentFlowCommandHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<ContentFlowResponse> Handle(CreateContentFlowCommand request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            if (await _repository.AlreadyExistAsync(cf =>
                cf.CurriculumId == request.CurriculumId &&
                cf.SubjectId == request.SubjectId &&
                cf.Name.ToLower() == request.Name.ToLower() &&
                cf.Description.ToLower() == request.Description.ToLower()
                ))
                throw new ContentFlowAlreadyExistException();

            var contentFlow = new ContentFlow
            {
                CurriculumId = request.CurriculumId,
                SubjectId = request.SubjectId,
                Name = request.Name,
                Description = request.Description
            };

            await _repository.AddAsync(contentFlow);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentFlowResponse>(contentFlow);
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

            if (await _repository.AlreadyExistAsync(cf =>
                cf.Id != request.Id &&
                cf.CurriculumId == request.CurriculumId &&
                cf.SubjectId == request.SubjectId &&
                cf.Name.ToLower() == request.Name.ToLower() &&
                cf.Description.ToLower() == request.Description.ToLower()
                ))
                throw new ContentFlowAlreadyExistException();

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
