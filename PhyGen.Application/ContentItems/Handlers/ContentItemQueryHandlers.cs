using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.ContentItems.Queries;
using PhyGen.Application.ContentItems.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Handlers
{
    public class GetContentItemByIdQueryHandler : IRequestHandler<GetContentItemByIdQuery, ContentItemResponse>
    {
        private readonly IContentItemRepository _contentItemRepository;

        public GetContentItemByIdQueryHandler(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }

        public async Task<ContentItemResponse> Handle(GetContentItemByIdQuery request, CancellationToken cancellationToken)
        {
            var contentItem = await _contentItemRepository.GetByIdAsync(request.Id) ?? throw new ContentItemNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemResponse>(contentItem);
        }
    }

    public class GetContentItemsByContentFlowIdQueryHandler : IRequestHandler<GetContentItemsByContentFlowIdQuery, List<ContentItemResponse>>
    {
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IContentFlowRepository _contentFlowRepository;

        public GetContentItemsByContentFlowIdQueryHandler(IContentItemRepository contentItemRepository, IContentFlowRepository contentFlowRepository)
        {
            _contentItemRepository = contentItemRepository;
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<List<ContentItemResponse>> Handle(GetContentItemsByContentFlowIdQuery request, CancellationToken cancellationToken)
        {
            if (await _contentFlowRepository.GetByIdAsync(request.ContentFlowId) == null)
                throw new ContentFlowNotFoundException();

            var contentItems = await _contentItemRepository.GetContentItemsByContentFlowIdAsync(request.ContentFlowId);
            if (contentItems == null || !contentItems.Any())
                throw new ContentItemNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ContentItemResponse>>(contentItems.OrderBy(ci => ci.Name));
        }
    }
}
