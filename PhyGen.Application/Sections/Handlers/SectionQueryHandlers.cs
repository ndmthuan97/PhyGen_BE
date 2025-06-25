using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Sections.Exceptions;
using PhyGen.Application.Sections.Queries;
using PhyGen.Application.Sections.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Handlers
{
    public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, SectionResponse>
    {
        private readonly ISectionRepository _sectionRepository;

        public GetSectionByIdQueryHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<SectionResponse> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var section = await _sectionRepository.GetByIdAsync(request.Id);

            if (section == null)
            {
                throw new SectionNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<SectionResponse>(section);
        }
    }

    public class GetSectionsByExamIdQueryHandler : IRequestHandler<GetSectionsByExamIdQuery, List<SectionResponse>>
    {
        private readonly ISectionRepository _sectionRepository;
        //private readonly IExamRepository _examRepository;

        public GetSectionsByExamIdQueryHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<List<SectionResponse>> Handle(GetSectionsByExamIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ExamId == Guid.Empty)
            {
                throw new Exception("Exam not found");
            }

            var sections = await _sectionRepository.GetSectionsByExamIdAsync(request.ExamId);
            if (sections == null)
            {
                throw new SectionNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<SectionResponse>>(sections);
        }
    }
}
