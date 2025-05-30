using MediatR;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class GetAllCurriculumsQueryHandler : IRequestHandler<GetAllCurriculumsQuery, List<CurriculumResponse>>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public GetAllCurriculumsQueryHandler(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<List<CurriculumResponse>> Handle(GetAllCurriculumsQuery request, CancellationToken cancellationToken)
        {
            var curriculums = await _curriculumRepository.GetAllAsync();

            var curriculumResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<CurriculumResponse>>(curriculums);

            return curriculumResponses;
        }
    }
}
