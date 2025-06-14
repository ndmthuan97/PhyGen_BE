using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixContentItems.Queries;
using PhyGen.Application.MatrixContentItems.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Handlers
{
    public class GetMatrixContentItemByIdQueryHandler : IRequestHandler<GetMatrixContentItemByIdQuery, MatrixContentItemResponse>
    {
        private readonly IMatrixContentItemRepository _matrixContentItemRepository;

        public GetMatrixContentItemByIdQueryHandler(IMatrixContentItemRepository matrixContentItemRepository)
        {
            _matrixContentItemRepository = matrixContentItemRepository;
        }

        public async Task<MatrixContentItemResponse> Handle(GetMatrixContentItemByIdQuery request, CancellationToken cancellationToken)
        {
            var matrixContentItem = await _matrixContentItemRepository.GetByIdAsync(request.MatrixContetnItemId)
                ?? throw new MatrixNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixContentItemResponse>(matrixContentItem);
        }
    }
}
