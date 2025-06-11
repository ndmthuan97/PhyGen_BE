using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectCurriculums.Exceptions;
using PhyGen.Application.SubjectCurriculums.Queries;
using PhyGen.Application.SubjectCurriculums.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Handlers
{
    public class GetSubjectCurriculumByIdQueryHandler : IRequestHandler<GetSubjectCurriculumByIdQuery, SubjectCurriculumResponse>
    {
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;

        public GetSubjectCurriculumByIdQueryHandler(ISubjectCurriculumRepository subjectCurriculumRepository)
        {
            _subjectCurriculumRepository = subjectCurriculumRepository;
        }

        public async Task<SubjectCurriculumResponse> Handle(GetSubjectCurriculumByIdQuery request, CancellationToken cancellationToken)
        {
            var subjectCurriculum = await _subjectCurriculumRepository.GetByIdAsync(request.SubjectCurriculumId)
                ?? throw new SubjectCurriculumNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<SubjectCurriculumResponse>(subjectCurriculum);
        }
    }

    public class GetSubjectCurriculumBySubjectIdAndCurriculumIdQueryHandler : IRequestHandler<GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery, SubjectCurriculumResponse>
    {
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;

        public GetSubjectCurriculumBySubjectIdAndCurriculumIdQueryHandler(ISubjectCurriculumRepository subjectCurriculumRepository)
        {
            _subjectCurriculumRepository = subjectCurriculumRepository;
        }

        public async Task<SubjectCurriculumResponse> Handle(GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery request, CancellationToken cancellationToken)
        {
            var subjectCurriculum = await _subjectCurriculumRepository.GetBySubjectIdAndCurriculumIdAsync(request.SubjectId, request.CurriculumId)
                ?? throw new SubjectCurriculumNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<SubjectCurriculumResponse>(subjectCurriculum);
        }
    }
}
