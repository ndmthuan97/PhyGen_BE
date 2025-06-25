using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixSectionDetails.Exceptions;
using PhyGen.Application.MatrixSectionDetails.Queries;
using PhyGen.Application.MatrixSectionDetails.Responses;
using PhyGen.Application.MatrixSections.Exceptions;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Handlers
{
    public class GetMatrixSectionDetailByIdQueryHandler : IRequestHandler<GetMatrixSectionDetailByIdQuery, MatrixSectionDetailResponse>
    {
        private readonly IMatrixSectionDetailRepository _matrixSectionDetailRepository;

        public GetMatrixSectionDetailByIdQueryHandler(IMatrixSectionDetailRepository matrixSectionDetailRepository)
        {
            _matrixSectionDetailRepository = matrixSectionDetailRepository;
        }

        public async Task<MatrixSectionDetailResponse> Handle(GetMatrixSectionDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var matrixSectionDetail = await _matrixSectionDetailRepository.GetByIdAsync(request.Id);

            if (matrixSectionDetail == null || matrixSectionDetail.DeletedAt.HasValue)
            {
                throw new MatrixSectionDetailNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixSectionDetailResponse>(matrixSectionDetail);
        }
    }

    public class GetMatrixSectionDetailsByMatrixSectionIdQueryHandler : IRequestHandler<GetMatrixSectionDetailsByMatrixSectionIdQuery, List<MatrixSectionDetailResponse>>
    {
        private readonly IMatrixSectionDetailRepository _matrixSectionDetailRepository;
        private readonly IMatrixSectionRepository _matrixSectionRepository;

        public GetMatrixSectionDetailsByMatrixSectionIdQueryHandler(IMatrixSectionDetailRepository matrixSectionDetailRepository, IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionDetailRepository = matrixSectionDetailRepository;
            _matrixSectionRepository = matrixSectionRepository;
        }

        public async Task<List<MatrixSectionDetailResponse>> Handle(GetMatrixSectionDetailsByMatrixSectionIdQuery request, CancellationToken cancellationToken)
        {
            var matrixSection = await _matrixSectionRepository.GetByIdAsync(request.MatrixSectionId);
            if (matrixSection == null || matrixSection.DeletedAt.HasValue)
            {
                throw new MatrixSectionNotFoundException();
            }

            var matrixSectionDetails = await _matrixSectionDetailRepository.GetMatrixSectionDetailsByMatrixSectionIdAsync(request.MatrixSectionId);
            
            matrixSectionDetails = matrixSectionDetails?.Where(msd => !msd.DeletedAt.HasValue).ToList();

            if (matrixSectionDetails == null || !matrixSectionDetails.Any())
            {
                throw new MatrixSectionDetailNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<MatrixSectionDetailResponse>>(matrixSectionDetails);
        }
    }
}
