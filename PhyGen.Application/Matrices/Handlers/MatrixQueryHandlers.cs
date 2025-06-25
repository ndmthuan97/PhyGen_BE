using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Matrices.Queries;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Handlers
{
    public class GetMatricesQueryHandlers : IRequestHandler<GetMatricesQuery, Pagination<MatrixResponse>>
    {
        private readonly IMatrixRepository _matrixRepository;

        public GetMatricesQueryHandlers(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Pagination<MatrixResponse>> Handle(GetMatricesQuery request, CancellationToken cancellationToken)
        {
            var matrices = await _matrixRepository.GetMatricesAsync(request.MatrixSpecParam);
            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<MatrixResponse>>(matrices);
        }
    }

    public class GetMatrixByIdQueryHandler : IRequestHandler<GetMatrixByIdQuery, MatrixResponse>
    {
        private readonly IMatrixRepository _matrixRepository;

        public GetMatrixByIdQueryHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<MatrixResponse> Handle(GetMatrixByIdQuery request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.Id);
            if (matrix == null || matrix.DeletedAt.HasValue)
                throw new MatrixNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixResponse>(matrix);
        }
    }
}
