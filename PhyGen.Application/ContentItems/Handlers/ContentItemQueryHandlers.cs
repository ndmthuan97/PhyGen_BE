using MediatR;
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
    public class GetAllContentItemsQueryHandler : IRequestHandler<GetAllContentItemsQuery, List<ContentItemResponse>>
    {
        private readonly IContentItemRepository _contentItemRepository;

        public GetAllContentItemsQueryHandler(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }

        public async Task<List<ContentItemResponse>> Handle(GetAllContentItemsQuery request, CancellationToken cancellationToken)
        {
            var contentItems = await _contentItemRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ContentItemResponse>>(contentItems);
        }
    }

    public class GetContentItemByIdQueryHandler : IRequestHandler<GetContentItemByIdQuery, ContentItemResponse>
    {
        private readonly IContentItemRepository _contentItemRepository;

        public GetContentItemByIdQueryHandler(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }

        public async Task<ContentItemResponse> Handle(GetContentItemByIdQuery request, CancellationToken cancellationToken)
        {
            var contentItem = await _contentItemRepository.GetByIdAsync(request.ContentItemId) ?? throw new ContentItemNotFoundException();
            
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemResponse>(contentItem);
        }
    }
}
