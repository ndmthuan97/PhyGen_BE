using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Handlers
{
    public class CreateContentItemCommandHandler : IRequestHandler<CreateContentItemCommand, Guid>
    {
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IContentFlowRepository _contentFlowRepository;

        public CreateContentItemCommandHandler(IContentItemRepository contentItemRepository, IContentFlowRepository contentFlowRepository)
        {
            _contentItemRepository = contentItemRepository;
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<Guid> Handle(CreateContentItemCommand request, CancellationToken cancellationToken)
        {
            if (await _contentFlowRepository.GetByIdAsync(request.ContentFlowId) == null)
                throw new ContentFlowNotFoundException();
            if (await _contentItemRepository.GetContentItemByContentFlowIdAndNameAsync(request.ContentFlowId, request.Name) != null)
                throw new ContentItemSameNameException();

            var contentItem = new ContentItem
            {
                ContentFlowId = request.ContentFlowId,
                Name = request.Name,
                LearningOutcome = request.LearningOutcome,
            };
            await _contentItemRepository.AddAsync(contentItem);
            return contentItem.Id;
        }
    }

    public class UpdateContentItemCommandHandler : IRequestHandler<UpdateContentItemCommand, Unit>
    {
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IContentFlowRepository _contentFlowRepository;

        public UpdateContentItemCommandHandler(IContentItemRepository contentItemRepository, IContentFlowRepository contentFlowRepository)
        {
            _contentItemRepository = contentItemRepository;
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<Unit> Handle(UpdateContentItemCommand request, CancellationToken cancellationToken)
        {
            if (await _contentFlowRepository.GetByIdAsync(request.ContentFlowId) == null)
                throw new ContentFlowNotFoundException();

            if (await _contentItemRepository.GetContentItemByContentFlowIdAndNameAsync(request.ContentFlowId, request.Name) != null)
                throw new ContentItemSameNameException();

            var contentItem = await _contentItemRepository.GetByIdAsync(request.Id);
            if (contentItem == null)
                throw new ContentItemNotFoundException();

            contentItem.Name = request.Name;
            contentItem.LearningOutcome = request.LearningOutcome;

            await _contentItemRepository.UpdateAsync(contentItem);
            return Unit.Value;
        }
    }
}
