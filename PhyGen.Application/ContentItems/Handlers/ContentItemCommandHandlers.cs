using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.ContentItems.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Handlers
{
    public class CreateContentItemCommandHandler : IRequestHandler<CreateContentItemCommand, ContentItemResponse>
    {
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IContentFlowRepository _contentFlowRepository;

        public CreateContentItemCommandHandler(IContentItemRepository contentItemRepository, IContentFlowRepository contentFlowRepository)
        {
            _contentItemRepository = contentItemRepository;
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<ContentItemResponse> Handle(CreateContentItemCommand request, CancellationToken cancellationToken)
        {
            var contentFlow = await _contentFlowRepository.GetByIdAsync(request.ContentFlowId);
            if (contentFlow == null || contentFlow.DeletedAt.HasValue)
                throw new ContentFlowNotFoundException();

            if (await _contentItemRepository.AlreadyExistAsync(c =>
                c.ContentFlowId == request.ContentFlowId &&
                c.Name.ToLower() == request.Name.ToLower() &&
                c.LearningOutcome.ToLower() == request.LearningOutcome.ToLower() &&
                c.DeletedAt == null
                ))
                throw new ContentItemAlreadyExistException();

            var contentItem = new ContentItem
            {
                ContentFlowId = request.ContentFlowId,
                Name = request.Name,
                LearningOutcome = request.LearningOutcome,
            };
            await _contentItemRepository.AddAsync(contentItem);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemResponse>(contentItem);
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
            var contentItem = await _contentItemRepository.GetByIdAsync(request.Id);
            if (contentItem == null)
                throw new ContentItemNotFoundException();

            var contentFlow = await _contentFlowRepository.GetByIdAsync(request.ContentFlowId);
            if (contentFlow == null || contentFlow.DeletedAt.HasValue)
                throw new ContentFlowNotFoundException();

            if (await _contentItemRepository.AlreadyExistAsync(c =>
                c.Id == request.Id &&
                c.ContentFlowId == request.ContentFlowId &&
                c.Name.ToLower() == request.Name.ToLower() &&
                c.LearningOutcome.ToLower() == request.LearningOutcome.ToLower() &&
                c.DeletedAt == null
                ))
                throw new ContentItemAlreadyExistException();

            contentItem.Name = request.Name;
            contentItem.LearningOutcome = request.LearningOutcome;

            await _contentItemRepository.UpdateAsync(contentItem);
            return Unit.Value;
        }
    }
    public class DeleteContentItemCommandHandler : IRequestHandler<DeleteContentItemCommand, Unit>
    {
        private readonly IContentItemRepository _contentItemRepository;
        public DeleteContentItemCommandHandler(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }
        public async Task<Unit> Handle(DeleteContentItemCommand request, CancellationToken cancellationToken)
        {
            var contentItem = await _contentItemRepository.GetByIdAsync(request.Id);
            if (contentItem == null || contentItem.DeletedAt.HasValue)
                throw new ContentItemNotFoundException();

            contentItem.DeletedAt = DateTime.UtcNow;

            await _contentItemRepository.UpdateAsync(contentItem);
            return Unit.Value;
        }
    }
}
