using MediatR;
using PhyGen.Application.Matrix.Commands;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrix.Handlers
{
    public class DeleteMatrixCommandHandler : IRequestHandler<DeleteMatrixCommand, Unit>
    {
        private readonly IMatrixRepository _matrixRepository;

        public DeleteMatrixCommandHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Unit> Handle(DeleteMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId) 
                ?? throw new KeyNotFoundException("Matrix not found.");

            matrix.DeletedBy = request.DeletedBy;
            matrix.DeletedAt = DateTime.UtcNow;

            await _matrixRepository.UpdateAsync(matrix);
            return Unit.Value;
        }
    }
}
