using MediatR;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class CreateContentFlowCommandHandler : IRequestHandler<CreateContentFlowCommand, int>
    {
        private readonly IContentFlowRepository _contentFlowRepository;

        public CreateContentFlowCommandHandler(IContentFlowRepository contentFlowRepository)
        {
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<int> Handle(CreateContentFlowCommand request, CancellationToken cancellationToken)
        {
            if (await _contentFlowRepository.GetContentFlowByNameAsync(request.Name) != null)
                throw new ContentFlowSameNameException();

            var contentFlow = new ContentFlow
            {
                Name = request.Name,
                Description = request.Description,
                SubjectId = request.SubjectId
            };

            await _contentFlowRepository.AddAsync(contentFlow);
            return contentFlow.Id;
        }
    }

    public class UpdateContentFlowCommandHandler : IRequestHandler<UpdateContentFlowCommand, Unit>
    {
        private readonly IContentFlowRepository _contentFlowRepository;

        public UpdateContentFlowCommandHandler(IContentFlowRepository contentFlowRepository)
        {
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<Unit> Handle(UpdateContentFlowCommand request, CancellationToken cancellationToken)
        {
            var contentFlow = await _contentFlowRepository.GetByIdAsync(request.Id) ?? throw new ContentFlowNotFoundException();
            
            if (await _contentFlowRepository.GetContentFlowByNameAsync(request.Name) != null &&
                contentFlow.Name != request.Name)
                throw new ContentFlowSameNameException();

            contentFlow.Name = request.Name;
            contentFlow.Description = request.Description;
            contentFlow.SubjectId = request.SubjectId;

            await _contentFlowRepository.UpdateAsync(contentFlow);
            return Unit.Value;
        }
    }
}
