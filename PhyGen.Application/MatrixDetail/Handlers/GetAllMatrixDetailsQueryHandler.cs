using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixDetail.Queries;
using PhyGen.Application.MatrixDetail.Responses;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Handlers
{
    public class GetAllMatrixDetailsQueryHandler : IRequestHandler<GetAllMatrixDetailsQuery, List<MatrixDetailResponse>>
    {
        private readonly IMatrixDetailRepository _matrixDetailRepository;

        public GetAllMatrixDetailsQueryHandler(IMatrixDetailRepository matrixDetailRepository)
        {
            _matrixDetailRepository = matrixDetailRepository;
        }

        public async Task<List<MatrixDetailResponse>> Handle(GetAllMatrixDetailsQuery request, CancellationToken cancellationToken)
        {
            var matrixDetails = _matrixDetailRepository.GetAllAsync();
            var matrixDetailResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixDetailResponse>>(matrixDetails);
            return matrixDetailResponses;
        }
    }
}
