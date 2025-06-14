using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Handlers
{
    public class CreateMatrixCommandHandler : IRequestHandler<CreateMatrixCommand, Guid>
    {
        private readonly IMatrixRepository _matrixRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateMatrixCommandHandler(
            IMatrixRepository matrixRepository,
            ISubjectRepository subjectRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _matrixRepository = matrixRepository;
            _subjectRepository = subjectRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<Guid> Handle(CreateMatrixCommand request, CancellationToken cancellationToken)
        {
            if (await _matrixRepository.GetBySubjectCurriculumIdAndExamCategoryIdAsync(request.SubjectId, request.ExamCategoryId) != null)
                throw new MatrixNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            //if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
            //    throw new ExamNotFoundException();

            var matrix = new Matrix
                {
                Name = request.Name,
                Description = request.Description,
                Grade = request.Grade,
                SubjectId = request.SubjectId,
                ExamCategoryId = request.ExamCategoryId,
                CreatedBy = request.CreatedBy
            };

            await _matrixRepository.AddAsync(matrix);
            return matrix.Id;
        }
    }

    public class UpdateMatrixCommandHandler : IRequestHandler<UpdateMatrixCommand, Unit>
    {
        private readonly IMatrixRepository _matrixRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        public UpdateMatrixCommandHandler(
            IMatrixRepository matrixRepository,
            ISubjectRepository subjectRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _matrixRepository = matrixRepository;
            _subjectRepository = subjectRepository;
            _examCategoryRepository = examCategoryRepository;
        }
        public async Task<Unit> Handle(UpdateMatrixCommand request, CancellationToken cancellationToken)
        {
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null)
                throw new MatrixNotFoundException();

            if (await _matrixRepository.GetBySubjectCurriculumIdAndExamCategoryIdAsync(request.SubjectId, request.ExamCategoryId) != null)
                throw new MatrixNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            //if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
            //    throw new ExamNotFoundException();

            matrix.Name = request.Name;
            matrix.Description = request.Description;
            matrix.Grade = request.Grade;
            matrix.SubjectId = request.SubjectId;
            matrix.ExamCategoryId = request.ExamCategoryId;
            matrix.UpdatedBy = request.UpdatedBy;

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
            var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId);
            if (matrix == null)
                throw new MatrixNotFoundException();

            await _matrixRepository.DeleteAsync(matrix);
            return Unit.Value;
        }
    }
}
