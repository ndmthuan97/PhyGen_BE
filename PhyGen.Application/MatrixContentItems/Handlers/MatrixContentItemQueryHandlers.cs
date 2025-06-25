using MediatR;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixContentItems.Exceptions;
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
            var matrixContentItem = await _matrixContentItemRepository.GetByIdAsync(request.Id);
            if (matrixContentItem == null)
            {
                throw new MatrixContentItemNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixContentItemResponse>(matrixContentItem);
        }
    }

    public class  GetMatrixContentItemsByMatrixIdAndContentItemIdQueryHandler : IRequestHandler<GetMatrixContentItemsByMatrixIdAndContentItemIdQuery, MatrixContentItemResponse>
    {
        private readonly IMatrixContentItemRepository _matrixContentItemRepository;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IContentItemRepository _contentItemRepository;

        public GetMatrixContentItemsByMatrixIdAndContentItemIdQueryHandler(
            IMatrixContentItemRepository matrixContentItemRepository,
            IMatrixRepository matrixRepository,
            IContentItemRepository contentItemRepository)
        {
            _matrixContentItemRepository = matrixContentItemRepository;
            _matrixRepository = matrixRepository;
            _contentItemRepository = contentItemRepository;
        }

        public async Task<MatrixContentItemResponse> Handle(GetMatrixContentItemsByMatrixIdAndContentItemIdQuery request, CancellationToken cancellationToken)
        {
            if (await _matrixRepository.GetByIdAsync(request.MatrixId) == null)
            {
                throw new MatrixNotFoundException();
            }

            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
            {
                throw new ContentItemNotFoundException();
            }

            var matrixContentItem = await _matrixContentItemRepository.GetMatrixContentItemsByMatrixIdAndContentItemIdAsync(request.MatrixId, request.ContentItemId);
            if (matrixContentItem == null)
            {
                throw new MatrixContentItemNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixContentItemResponse>(matrixContentItem);
        }
    }
}
