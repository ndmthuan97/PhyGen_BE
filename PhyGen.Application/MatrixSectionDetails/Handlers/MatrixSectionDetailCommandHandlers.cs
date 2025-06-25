using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixSectionDetails.Commands;
using PhyGen.Application.MatrixSectionDetails.Exceptions;
using PhyGen.Application.MatrixSectionDetails.Responses;
using PhyGen.Application.MatrixSections.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Handlers
{
    public class CreateMatrixSectionDetailCommandHandler : IRequestHandler<CreateMatrixSectionDetailCommand, MatrixSectionDetailResponse>
    {
        private readonly IMatrixSectionDetailRepository _matrixSectionDetailRepository;
        private readonly IMatrixSectionRepository _matrixSectionRepository;
        //private readonly ISectionRepository _sectionRepository;

        public CreateMatrixSectionDetailCommandHandler(
            IMatrixSectionDetailRepository matrixSectionDetailRepository,
            //ISectionRepository sectionRepository,
            IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionDetailRepository = matrixSectionDetailRepository;
            _matrixSectionRepository = matrixSectionRepository;
            //_sectionRepository = sectionRepository;
        }

        public async Task<MatrixSectionDetailResponse> Handle(CreateMatrixSectionDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixSection = await _matrixSectionRepository.GetByIdAsync(request.MatrixSectionId);
            if (matrixSection == null || matrixSection.DeletedAt.HasValue)
            {
                throw new MatrixSectionNotFoundException();
            }

            //var section = await _sectionRepository.GetByIdAsync(request.SectionId);
            //if (section == null || section.DeletedAt.HasValue)
            //{
            //    throw new SectionNotFoundException();
            //}

            var isExist = await _matrixSectionDetailRepository.AlreadyExistAsync(msd =>
                msd.MatrixSectionId == request.MatrixSectionId &&
                msd.SectionId == request.SectionId &&
                msd.Title.ToLower() == request.Title.ToLower() &&
                msd.Level == request.Level &&
                msd.Type == request.Type &&
                msd.Quantity == request.Quantity &&
                msd.DeletedAt == null
            );
            if (isExist)
                throw new MatrixSectionDetailAlreadyExistException();

            var matrixSectionDetail = new MatrixSectionDetail
            {
                MatrixSectionId = request.MatrixSectionId,
                SectionId = request.SectionId,
                Title = request.Title,
                Description = request.Description,
                Level = request.Level,
                Type = request.Type,
                Quantity = request.Quantity
            };

            await _matrixSectionDetailRepository.AddAsync(matrixSectionDetail);
            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixSectionDetailResponse>(matrixSectionDetail);
        }
    }

    public class UpdateMatrixSectionDetailCommandHandler : IRequestHandler<UpdateMatrixSectionDetailCommand, Unit>
    {
        private readonly IMatrixSectionDetailRepository _matrixSectionDetailRepository;
        private readonly IMatrixSectionRepository _matrixSectionRepository;
        //private readonly ISectionRepository _sectionRepository;

        public UpdateMatrixSectionDetailCommandHandler(
            IMatrixSectionDetailRepository matrixSectionDetailRepository,
            //ISectionRepository sectionRepository,
            IMatrixSectionRepository matrixSectionRepository)
        {
            _matrixSectionDetailRepository = matrixSectionDetailRepository;
            _matrixSectionRepository = matrixSectionRepository;
            //_sectionRepository = sectionRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixSectionDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixSectionDetail = await _matrixSectionDetailRepository.GetByIdAsync(request.Id);
            if (matrixSectionDetail == null || matrixSectionDetail.DeletedAt.HasValue)
            {
                throw new MatrixSectionDetailNotFoundException();
            }

            var matrixSection = await _matrixSectionRepository.GetByIdAsync(request.MatrixSectionId);
            if (matrixSection == null || matrixSection.DeletedAt.HasValue)
            {
                throw new MatrixSectionNotFoundException();
            }

            //var section = await _sectionRepository.GetByIdAsync(request.SectionId);
            //if (section == null || section.DeletedAt.HasValue)
            //{
            //    throw new SectionNotFoundException();
            //}

            var isExist = await _matrixSectionDetailRepository.AlreadyExistAsync(msd =>
                msd.MatrixSectionId == request.MatrixSectionId &&
                msd.SectionId == request.SectionId &&
                msd.Title.ToLower() == request.Title.ToLower() &&
                msd.Level == request.Level &&
                msd.Type == request.Type &&
                msd.Quantity == request.Quantity &&
                msd.Id != request.Id &&
                msd.DeletedAt == null
            );
            if (isExist)
                throw new MatrixSectionDetailAlreadyExistException();

            matrixSectionDetail.MatrixSectionId = request.MatrixSectionId;
            matrixSectionDetail.SectionId = request.SectionId;
            matrixSectionDetail.Title = request.Title;
            matrixSectionDetail.Description = request.Description;
            matrixSectionDetail.Level = request.Level;
            matrixSectionDetail.Type = request.Type;
            matrixSectionDetail.Quantity = request.Quantity;

            await _matrixSectionDetailRepository.UpdateAsync(matrixSectionDetail);
            return Unit.Value;
        }
    }

    public class DeleteMatrixSectionDetailCommandHandler : IRequestHandler<DeleteMatrixSectionDetailCommand, Unit>
    {
        private readonly IMatrixSectionDetailRepository _matrixSectionDetailRepository;

        public DeleteMatrixSectionDetailCommandHandler(IMatrixSectionDetailRepository matrixSectionDetailRepository)
        {
            _matrixSectionDetailRepository = matrixSectionDetailRepository;
        }

        public async Task<Unit> Handle(DeleteMatrixSectionDetailCommand request, CancellationToken cancellationToken)
        {
            var matrixSectionDetail = await _matrixSectionDetailRepository.GetByIdAsync(request.Id);
            if (matrixSectionDetail == null || matrixSectionDetail.DeletedAt.HasValue)
            {
                throw new MatrixSectionDetailNotFoundException();
            }

            await _matrixSectionDetailRepository.DeleteAsync(matrixSectionDetail);
            return Unit.Value;
        }
    }
}
