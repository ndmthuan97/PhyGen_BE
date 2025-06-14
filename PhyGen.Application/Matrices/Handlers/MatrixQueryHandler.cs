using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Matrices.Queries;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Handlers
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
            var matrices = await _matrixRepository.GetAllAsync();
            var activeMatrices = matrices.Where(m => m.DeletedBy == null).ToList();
            return AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixResponse>>(activeMatrices);
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
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId) 
                ?? throw new MatrixNotFoundException();

            if (matrix.DeletedBy != null)
                throw new MatrixNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixResponse>(matrix);
        }
    }
}
