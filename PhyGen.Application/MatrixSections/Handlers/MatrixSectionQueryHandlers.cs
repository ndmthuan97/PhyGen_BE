using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixSections.Exceptions;
using PhyGen.Application.MatrixSections.Queries;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Handlers
{
    public class GetMatrixSectionByIdQueryHandler : IRequestHandler<GetMatrixSectionByIdQuery, List<MatrixSectionResponse>>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;

        public GetMatrixSectionByIdQueryHandler(IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
        }

        public async Task<List<MatrixSectionResponse>> Handle(GetMatrixSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var matrixSections = await _matrixSectionRepository.GetAllAsync();

            if (matrixSections == null || !matrixSections.Any())
                throw new MatrixSectionNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixSectionResponse>>(matrixSections);
        }
    }

    public class GetMatrixSectionsByMatrixIdQueryHandler : IRequestHandler<GetMatrixSectionsByMatrixIdQuery, List<MatrixSectionResponse>>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;
        private readonly IMatrixRepository _matrixRepository;

        public GetMatrixSectionsByMatrixIdQueryHandler(IMatrixSectionRepository matrixSectionRepository, IMatrixRepository matrixRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
            _matrixRepository = matrixRepository;
        }

        public async Task<List<MatrixSectionResponse>> Handle(GetMatrixSectionsByMatrixIdQuery request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null || matrix.DeletedAt.HasValue)
                throw new MatrixNotFoundException();

            var matrixSections = await _matrixSectionRepository.GetMatrixSectionsByMatrixIdAsync(request.MatrixId);

            matrixSections = matrixSections?.Where(ms => !ms.DeletedAt.HasValue).ToList();

            if (matrixSections == null || !matrixSections.Any())
                throw new MatrixSectionNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixSectionResponse>>(matrixSections);
        }
    }
}
