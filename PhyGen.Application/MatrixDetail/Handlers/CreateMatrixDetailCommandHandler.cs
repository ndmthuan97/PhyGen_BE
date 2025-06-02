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
    public class CreateMatrixDetailCommandHandler : IRequestHandler<CreateMatrixDetailCommand, Guid>
    {
        private readonly IMatrixDetailRepository _matrixDetailRepository;

        public CreateMatrixDetailCommandHandler(IMatrixDetailRepository matrixDetailRepository)
        {
            _matrixDetailRepository = matrixDetailRepository;
        }

        public async Task<Guid> Handle(CreateMatrixDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixDetail = new PhyGen.Domain.Entities.MatrixDetail
            {
                Id = Guid.NewGuid(),
                MatrixId = request.MatrixId,
                ChapterId = request.ChapterId,
                Level = request.Level,
                Quantity = request.Quantity,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            matrixDetail = _matrixDetailRepository.AddAsync(matrixDetail).Result;
            return matrixDetail.Id;
        }
    }
}
