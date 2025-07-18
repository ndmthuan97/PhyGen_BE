using MediatR;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Handlers
{
    public class CreateMatrixCommandHandler : IRequestHandler<CreateMatrixCommand, MatrixResponse>
    {
        private readonly IMatrixRepository _matrixRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateMatrixCommandHandler(IMatrixRepository matrixRepository, ISubjectRepository subjectRepository, IExamCategoryRepository examCategoryRepository)
        {
            _matrixRepository = matrixRepository;
            _subjectRepository = subjectRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<MatrixResponse> Handle(CreateMatrixCommand request, CancellationToken cancellationToken)
        {
            var category = await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId);
            if (category == null || category.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            var isExist = await _matrixRepository.AlreadyExistAsync(m =>
                m.SubjectId == request.SubjectId &&
                m.ExamCategoryId == request.ExamCategoryId &&
                m.Name.ToLower() == request.Name.ToLower() &&
                m.DeletedAt == null
            );
            if (isExist)
                throw new MatrixAlreadyExistException();

            var matrix = new Matrix
            {
                SubjectId = request.SubjectId,
                ExamCategoryId = request.ExamCategoryId,
                Name = request.Name,
                Description = request.Description,
                TotalQuestionCount = request.TotalQuestionCount,
                Grade = request.Grade,
                Year = request.Year,
                ImgUrl = request.ImgUrl,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };
            await _matrixRepository.AddAsync(matrix);
            return AppMapper<CoreMappingProfile>.Mapper.Map<MatrixResponse>(matrix);
        }
    }

    public class UpdateMatrixCommandHandler : IRequestHandler<UpdateMatrixCommand, Unit>
    {
        private readonly IMatrixRepository _matrixRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public UpdateMatrixCommandHandler(IMatrixRepository matrixRepository, ISubjectRepository subjectRepository, IExamCategoryRepository examCategoryRepository)
        {
            _matrixRepository = matrixRepository;
            _subjectRepository = subjectRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<Unit> Handle(UpdateMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.Id);
            if (matrix == null || matrix.DeletedAt.HasValue)
                throw new MatrixNotFoundException();

            var category = await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId);
            if (category == null || category.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            var isExist = await _matrixRepository.AlreadyExistAsync(m =>
                m.Id != request.Id &&
                m.SubjectId == request.SubjectId &&
                m.ExamCategoryId == request.ExamCategoryId &&
                m.Name.ToLower() == request.Name.ToLower() &&
                m.DeletedAt == null
            );
            if (isExist)
                throw new MatrixAlreadyExistException();

            matrix.SubjectId = request.SubjectId;
            matrix.ExamCategoryId = request.ExamCategoryId;
            matrix.Name = request.Name;
            matrix.Description = request.Description;
            matrix.TotalQuestionCount = request.TotalQuestionCount;
            matrix.Grade = request.Grade;
            matrix.Year = request.Year;
            matrix.ImgUrl = request.ImgUrl;

            await _matrixRepository.UpdateAsync(matrix);
            return Unit.Value;
        }
    }

    public class DeleteMatrixCommandHandler : IRequestHandler<DeleteMatrixCommand, Unit>
    {
        private readonly IMatrixRepository _matrixRepository;

        public DeleteMatrixCommandHandler(IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Unit> Handle(DeleteMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.Id);
            if (matrix == null || matrix.DeletedAt.HasValue)
                throw new MatrixNotFoundException();

            matrix.DeletedAt = DateTime.UtcNow;

            await _matrixRepository.UpdateAsync(matrix);
            return Unit.Value;
        }
    }
}
