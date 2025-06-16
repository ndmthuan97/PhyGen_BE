using MediatR;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
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

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<CurriculumResponse>>(curriculums);
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
