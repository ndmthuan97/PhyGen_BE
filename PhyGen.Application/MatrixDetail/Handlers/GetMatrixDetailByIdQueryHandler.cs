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
    public class GetMatrixDetailByIdQueryHandler : IRequestHandler<GetMatrixDetailByIdQuery, MatrixDetailResponse>
    {
        private readonly IMatrixDetailRepository _matrixDetailRepository;

        public GetMatrixDetailByIdQueryHandler(IMatrixDetailRepository matrixDetailRepository)
        {
            _matrixDetailRepository = matrixDetailRepository;
        }

        public async Task<MatrixDetailResponse> Handle(GetMatrixDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var matrixDetail = await _matrixDetailRepository.GetByIdAsync(request.MatrixDetailId)
                ?? throw new KeyNotFoundException("Matrix detail not found.");

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixDetailResponse>(matrixDetail);
        }
    }
}
