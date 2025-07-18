using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixSections.Exceptions;
using PhyGen.Application.MatrixSections.Queries;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
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

    public class GetMatrixSectionsByMatrixIdQueryHandler : IRequestHandler<GetMatrixSectionsByMatrixIdQuery, Pagination<MatrixSectionResponse>>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;

        public GetMatrixSectionsByMatrixIdQueryHandler(IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
        }

        public async Task<Pagination<MatrixSectionResponse>> Handle(GetMatrixSectionsByMatrixIdQuery request, CancellationToken cancellationToken)
        {
            var matrixSections = await _matrixSectionRepository.GetMatrixSectionsByMatrixIdAsync(request.param);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<MatrixSectionResponse>>(matrixSections);
        }
    }
}
