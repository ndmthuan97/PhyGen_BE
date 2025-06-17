using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentFlows.Queries;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class GetContentFlowByIdQueryHandler : IRequestHandler<GetContentFlowByIdQuery, ContentFlowResponse>
    {
        private readonly IContentFlowRepository _repository;
        public GetContentFlowByIdQueryHandler(IContentFlowRepository repository)
        {
            _repository = repository;
        }
        public async Task<ContentFlowResponse> Handle(GetContentFlowByIdQuery request, CancellationToken cancellationToken)
        {
            var contentFlow = await _repository.GetByIdAsync(request.Id) ?? throw new ContentFlowNotFoundException();
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentFlowResponse>(contentFlow);
        }
    }

    public class GetContentFlowsByCurriculumIdQueryHandler : IRequestHandler<GetContentFlowsByCurriculumIdQuery, List<ContentFlowResponse>>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;

        public GetContentFlowsByCurriculumIdQueryHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<List<ContentFlowResponse>> Handle(GetContentFlowsByCurriculumIdQuery request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            var contentFlows = await _repository.GetContentFlowsByCurriculumIdAsync(request.CurriculumId);

            if (contentFlows == null || !contentFlows.Any())
                throw new ContentFlowNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ContentFlowResponse>>(contentFlows.OrderBy(cf => cf.Name));
        }
    }
}
