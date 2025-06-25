using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.MatrixSections.Commands;
using PhyGen.Application.MatrixSections.Exceptions;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Handlers
{
    public class CreateMatrixSectionCommandHandler : IRequestHandler<CreateMatrixSectionCommand, MatrixSectionResponse>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;
        private readonly IMatrixRepository _matrixRepository;

        public CreateMatrixSectionCommandHandler(IMatrixSectionRepository matrixSectionRepository, IMatrixRepository matrixRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
            _matrixRepository = matrixRepository;
        }

        public async Task<MatrixSectionResponse> Handle(CreateMatrixSectionCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null || matrix.DeletedAt.HasValue)
            {
                throw new MatrixNotFoundException();
            }

            var isExist = await _matrixSectionRepository.AlreadyExistAsync(ms =>
                ms.MatrixId == request.MatrixId &&
                ms.Title.ToLower() == request.Title.ToLower() &&
                ms.Score == request.Score &&
                ms.DeletedAt == null
            );
            if (isExist)
                throw new MatrixSectionAlreadyExistException();

            var matrixSection = new MatrixSection
            {
                MatrixId = request.MatrixId,
                Title = request.Title,
                Score = request.Score,
                Description = request.Description
            };
            await _matrixSectionRepository.AddAsync(matrixSection);
            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixSectionResponse>(matrixSection);
        }
    }

    public class UpdateMatrixSectionCommandHandler : IRequestHandler<UpdateMatrixSectionCommand, Unit>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;
        private readonly IMatrixRepository _matrixRepository;

        public UpdateMatrixSectionCommandHandler(IMatrixSectionRepository matrixSectionRepository, IMatrixRepository matrixRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
            _matrixRepository = matrixRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixSectionCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null || matrix.DeletedAt.HasValue)
            {
                throw new MatrixNotFoundException();
            }

            var matrixSection = await _matrixSectionRepository.GetByIdAsync(request.Id);
            if (matrixSection == null || matrixSection.DeletedAt.HasValue)
            {
                throw new MatrixSectionNotFoundException();
            }

            matrixSection.MatrixId = request.MatrixId;
            matrixSection.Title = request.Title;
            matrixSection.Score = request.Score;
            matrixSection.Description = request.Description;

            await _matrixSectionRepository.UpdateAsync(matrixSection);
            return Unit.Value;
        }
    }

    public class DeleteMatrixSectionCommandHandler : IRequestHandler<DeleteMatrixSectionCommand, Unit>
    {
        private readonly IMatrixSectionRepository _matrixSectionRepository;

        public DeleteMatrixSectionCommandHandler(IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionRepository = matrixSectionRepository;
        }

        public async Task<Unit> Handle(DeleteMatrixSectionCommand request, CancellationToken cancellationToken)
        {
            var matrixSection = await _matrixSectionRepository.GetByIdAsync(request.Id);
            if (matrixSection == null || matrixSection.DeletedAt.HasValue)
            {
                throw new MatrixSectionNotFoundException();
            }

            await _matrixSectionRepository.DeleteAsync(matrixSection);
            return Unit.Value;
        }
    }
}
