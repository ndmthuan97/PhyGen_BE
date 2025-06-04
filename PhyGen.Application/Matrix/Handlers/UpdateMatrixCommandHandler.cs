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
    public class UpdateMatrixCommandHandler : IRequestHandler<UpdateMatrixCommand, Unit>
    {
        private readonly IMatrixRepository _matrixRepository;

        public UpdateMatrixCommandHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null)
            {
                throw new KeyNotFoundException("Matrix not found.");
            }

            matrix.Name = request.Name;
            matrix.Description = request.Description;
            matrix.Grade = request.Grade;
            matrix.UserId = request.UserId;
            matrix.UpdatedBy = request.UpdatedBy;
            matrix.UpdatedAt = DateTime.UtcNow;

            await _matrixRepository.UpdateAsync(matrix);
            return Unit.Value;
        }
    }
}
