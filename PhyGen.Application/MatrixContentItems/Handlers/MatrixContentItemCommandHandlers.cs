using MediatR;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixContentItems.Commands;
using PhyGen.Application.MatrixContentItems.Exceptions;
using PhyGen.Application.MatrixContentItems.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Handlers
{
    public class CreateMatrixContentItemCommandHandler : IRequestHandler<CreateMatrixContentItemCommand, MatrixContentItemResponse>
    {
        private readonly IMatrixContentItemRepository _matrixContentItemRepository;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IContentItemRepository _contentItemRepository;

        public CreateMatrixContentItemCommandHandler(
            IMatrixContentItemRepository matrixContentItemRepository,
            IMatrixRepository matrixRepository,
            IContentItemRepository contentItemRepository)
        {
            _matrixContentItemRepository = matrixContentItemRepository;
            _matrixRepository = matrixRepository;
            _contentItemRepository = contentItemRepository;
        }

        public async Task<MatrixContentItemResponse> Handle(CreateMatrixContentItemCommand request, CancellationToken cancellationToken)
        {
            if (await _matrixRepository.GetByIdAsync(request.MatrixId) == null)
            {
                throw new MatrixNotFoundException();
            }

            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
            {
                throw new ContentItemNotFoundException();
            }

            if (await _matrixContentItemRepository.GetMatrixContentItemsByMatrixIdAndContentItemIdAsync(request.MatrixId, request.ContentItemId) != null)
            {
                throw new MatrixContentItemAlreadyExistException();
            }

            var matrixContentItem = new MatrixContentItem
            {
                MatrixId = request.MatrixId,
                ContentItemId = request.ContentItemId
            };

            await _matrixContentItemRepository.AddAsync(matrixContentItem);
            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixContentItemResponse>(matrixContentItem);
        }
    }

    public class UpdateMatrixContentItemCommandHandler : IRequestHandler<UpdateMatrixContentItemCommand, Unit>
    {
        private readonly IMatrixContentItemRepository _matrixContentItemRepository;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IContentItemRepository _contentItemRepository;

        public UpdateMatrixContentItemCommandHandler(
            IMatrixContentItemRepository matrixContentItemRepository,
            IMatrixRepository matrixRepository,
            IContentItemRepository contentItemRepository)
        {
            _matrixContentItemRepository = matrixContentItemRepository;
            _matrixRepository = matrixRepository;
            _contentItemRepository = contentItemRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixContentItemCommand request, CancellationToken cancellationToken)
        {
            var matrixContentItem = await _matrixContentItemRepository.GetByIdAsync(request.Id);
            if (matrixContentItem == null)
            {
                throw new MatrixContentItemNotFoundException();
            }

            if (await _matrixRepository.GetByIdAsync(request.MatrixId) == null)
            {
                throw new MatrixNotFoundException();
            }

            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
            {
                throw new ContentItemNotFoundException();
            }

            matrixContentItem.MatrixId = request.MatrixId;
            matrixContentItem.ContentItemId = request.ContentItemId;

            await _matrixContentItemRepository.UpdateAsync(matrixContentItem);
            return Unit.Value;
        }
    }
}
