using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentFlows.Queries;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class GetAllContentFlowsQueryHandler : IRequestHandler<GetAllContentFlowsQuery, List<ContentFlowResponse>>
    {
        private readonly IContentFlowRepository _contentFlowRepository;

        public GetAllContentFlowsQueryHandler(IContentFlowRepository contentFlowRepository)
        {
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<List<ContentFlowResponse>> Handle(GetAllContentFlowsQuery request, CancellationToken cancellationToken)
        {
            var contentFlows = await _contentFlowRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ContentFlowResponse>>(contentFlows);
        }
    }

    public class GetContentFlowByIdQueryHandler : IRequestHandler<GetContentFlowByIdQuery, ContentFlowResponse>
    {
        private readonly IContentFlowRepository _contentFlowRepository;

        public GetContentFlowByIdQueryHandler(IContentFlowRepository contentFlowRepository)
        {
            _contentFlowRepository = contentFlowRepository;
        }

        public async Task<ContentFlowResponse> Handle(GetContentFlowByIdQuery request, CancellationToken cancellationToken)
        {
            var contentFlow = await _contentFlowRepository.GetByIdAsync(request.ContentFlowId) ?? throw new ContentFlowNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentFlowResponse>(contentFlow);
        }
    }
}
