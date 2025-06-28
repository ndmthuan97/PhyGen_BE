using MediatR;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentFlows.Queries;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Subjects.Exceptions;
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
            var contentFlow = await _repository.GetByIdAsync(request.Id);

            if (contentFlow == null || contentFlow.DeletedAt.HasValue)
                throw new ContentFlowNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentFlowResponse>(contentFlow);
        }
    }

    public class GetContentFlowsByCurriculumIdAndSubjectIdQueryHandler : IRequestHandler<GetContentFlowsByCurriculumIdAndSubjectIdQuery, List<ContentFlowResponse>>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ISubjectRepository _subjectRepository;

        public GetContentFlowsByCurriculumIdAndSubjectIdQueryHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository, ISubjectRepository subjectRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<List<ContentFlowResponse>> Handle(GetContentFlowsByCurriculumIdAndSubjectIdQuery request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            var contentFlows = await _repository.GetContentFlowsByCurriculumIdAndSubjectIdAsync(request.CurriculumId, request.SubjectId);

            contentFlows = contentFlows?.Where(cf => !cf.DeletedAt.HasValue).ToList();

            if (contentFlows == null || !contentFlows.Any())
                throw new ContentFlowNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ContentFlowResponse>>(contentFlows.OrderBy(cf => cf.OrderNo));
        }
    }
}
