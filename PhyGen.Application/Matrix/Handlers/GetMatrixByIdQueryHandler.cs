using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrix.Queries;
using PhyGen.Application.Matrix.Responses;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrix.Handlers
{
    public class GetMatrixByIdQueryHandler : IRequestHandler<GetMatrixByIdQuery, MatrixResponse>
    {
        private readonly IMatrixRepository _matrixRepository;

        public GetMatrixByIdQueryHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<MatrixResponse> Handle(GetMatrixByIdQuery request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId) 
                ?? throw new KeyNotFoundException("Matrix not found.");

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixResponse>(matrix);
        }
    }
}
