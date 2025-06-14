using MediatR;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixContentItems.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Handlers
{
    public class CreateMatrixContentItemCommandHandler : IRequestHandler<CreateMatrixContentItemCommand, int>
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

        public async Task<int> Handle(CreateMatrixContentItemCommand request, CancellationToken cancellationToken)
        {
            var matrixContentItem = new MatrixContentItem
            {
                MatrixId = request.MatrixId,
                ContentItemId = request.ContentItemId
            };

            await _matrixContentItemRepository.AddAsync(matrixContentItem);
            return matrixContentItem.Id;
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
            var matrixContentItem = new MatrixContentItem
            {
                Id = request.Id,
                MatrixId = request.MatrixId,
                ContentItemId = request.ContentItemId
            };

            await _matrixContentItemRepository.UpdateAsync(matrixContentItem);
            return Unit.Value;
        }
    }
}
