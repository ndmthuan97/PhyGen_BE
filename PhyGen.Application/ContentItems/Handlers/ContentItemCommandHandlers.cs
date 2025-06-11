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
            if (await _contentItemRepository.GetContentItemByTitleAsync(request.Title) != null)
             throw new ContentItemSameNameException();

            if (await _contentFlowRepository.GetByIdAsync(request.ContentFlowId) == null)
                throw new ContentFlowNotFoundException();

            var contentItem = new ContentItem
            {
                ContentFlowId = request.ContentFlowId,
                Title = request.Title,
                LearningOutcome = request.LearningOutcome,
                CreatedAt = DateTime.UtcNow
            };

            await _contentItemRepository.AddAsync(contentItem);
            return contentItem.Id;
        }
    }

    public class UpdateContentItemCommandHandler : IRequestHandler<UpdateContentItemCommand, Unit>
    {
        private readonly IContentItemRepository _contentItemRepository;
        public UpdateContentItemCommandHandler(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }
        public async Task<Unit> Handle(UpdateContentItemCommand request, CancellationToken cancellationToken)
        {
            var contentItem = await _contentItemRepository.GetByIdAsync(request.Id) ?? throw new ContentItemNotFoundException();
            
            if (await _contentItemRepository.GetContentItemByTitleAsync(request.Title) != null && contentItem.Title != request.Title)
                throw new ContentItemSameNameException();

            contentItem.ContentFlowId = request.ContentFlowId;
            contentItem.Title = request.Title;
            contentItem.LearningOutcome = request.LearningOutcome;

            await _contentItemRepository.UpdateAsync(contentItem);
            return Unit.Value;
        }
    }
}
