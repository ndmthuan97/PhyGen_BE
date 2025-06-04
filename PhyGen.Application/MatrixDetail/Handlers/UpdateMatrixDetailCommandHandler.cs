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
    public class UpdateMatrixDetailCommandHandler : IRequestHandler<UpdateMatrixDetailCommand, Unit>
    {
        private readonly IMatrixDetailRepository _matrixDetailRepository;

        public UpdateMatrixDetailCommandHandler(IMatrixDetailRepository matrixDetailRepository)
        {
            _matrixDetailRepository = matrixDetailRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixDetail = await _matrixDetailRepository.GetByIdAsync(request.MatrixDetailId);

            if (matrixDetail == null)
            {
                throw new KeyNotFoundException("Matrix detail not found.");
            }

            matrixDetail.MatrixId = request.MatrixId;
            matrixDetail.ChapterId = request.ChapterId;
            matrixDetail.Level = request.Level;
            matrixDetail.Quantity = request.Quantity;
            matrixDetail.UpdatedBy = request.UpdatedBy;
            matrixDetail.UpdatedAt = DateTime.UtcNow;

            await _matrixDetailRepository.UpdateAsync(matrixDetail);
            return Unit.Value;
        }
    }
}
