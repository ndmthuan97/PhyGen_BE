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
    public class GetAllMatricesQueryHandler : IRequestHandler<GetAllMatricesQuery, List<MatrixResponse>>
    {
        private readonly IMatrixRepository _matrixRepository;

        public GetAllMatricesQueryHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<List<MatrixResponse>> Handle(GetAllMatricesQuery request, CancellationToken cancellationToken)
        {
            var matrices = _matrixRepository.GetAllAsync();
            var matrixResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixResponse>>(matrices);
            return matrixResponses;
        }
    }
}
