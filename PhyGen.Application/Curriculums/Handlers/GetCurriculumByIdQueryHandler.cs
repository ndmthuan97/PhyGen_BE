using MediatR;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class GetCurriculumByIdQueryHandler : IRequestHandler<GetCurriculumByIdQuery, CurriculumResponse>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public GetCurriculumByIdQueryHandler(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<CurriculumResponse> Handle(GetCurriculumByIdQuery request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId) ?? throw new CurriculumNotFoundException();
            return AppMapper<CoreMappingProfile>.Mapper.Map<CurriculumResponse>(curriculum);
        }
    }
}
