using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Application.Subjects.Queries;
using PhyGen.Application.Subjects.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Subjects.Handlers
{
    public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, List<SubjectResponse>>
    {
        private readonly ISubjectRepository _subjectRepository;
        public GetAllSubjectsQueryHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<List<SubjectResponse>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<SubjectResponse>>(subjects.OrderBy(s => s.Name));
        }
    }

    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectResponse>
    {
        private readonly ISubjectRepository _subjectRepository;
        public GetSubjectByIdQueryHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<SubjectResponse> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await _subjectRepository.GetByIdAsync(request.SubjectId) ?? throw new SubjectNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<SubjectResponse>(subject);
        }
    }
}
