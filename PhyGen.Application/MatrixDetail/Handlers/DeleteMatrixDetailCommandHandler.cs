using MediatR;
using PhyGen.Application.MatrixDetail.Commands;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Handlers
{
    public class DeleteMatrixDetailCommandHandler : IRequestHandler<DeleteMatrixDetailCommand, Unit>
    {
        private readonly IMatrixDetailRepository _matrixDetailRepository;

        public DeleteMatrixDetailCommandHandler(IMatrixDetailRepository matrixDetailRepository)
        {
            _matrixDetailRepository = matrixDetailRepository;
        }

        public async Task<Unit> Handle(DeleteMatrixDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixDetail = await _matrixDetailRepository.GetByIdAsync(request.MatrixDetailId) 
                ?? throw new KeyNotFoundException("Matrix detail not found.");

            matrixDetail.DeletedBy = request.DeletedBy;
            matrixDetail.DeletedAt = DateTime.UtcNow;

            await _matrixDetailRepository.UpdateAsync(matrixDetail);
            return Unit.Value;
        }
    }
}
