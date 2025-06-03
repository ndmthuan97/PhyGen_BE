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
    public class CreateMatrixCommandHandler : IRequestHandler<CreateMatrixCommand, Guid>
    {
        private readonly IMatrixRepository _matrixRepository;

        public CreateMatrixCommandHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Guid> Handle(CreateMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = new PhyGen.Domain.Entities.Matrix
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Grade = request.Grade,
                UserId = request.UserId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            matrix = await _matrixRepository.AddAsync(matrix);
            return matrix.Id;
        }
    }
}
