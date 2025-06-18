using MediatR;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class GetCurriculumsQueryHandler : IRequestHandler<GetCurriculumsQuery, Pagination<CurriculumResponse>>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public GetCurriculumsQueryHandler(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Pagination<CurriculumResponse>> Handle(GetCurriculumsQuery request, CancellationToken cancellationToken)
        {
            var curriculums = await _curriculumRepository.GetCurriculumsAsync(request.CurriculumSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<CurriculumResponse>>(curriculums);
        }
    }

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
