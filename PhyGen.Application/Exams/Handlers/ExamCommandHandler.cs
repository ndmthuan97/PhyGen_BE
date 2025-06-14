using MediatR;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.SubjectCurriculums.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Guid>
    {
        private readonly IExamRepository _examRepository;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;

        public CreateExamCommandHandler(
            IExamRepository examRepository, 
            IMatrixRepository matrixRepository,
            IExamCategoryRepository examCategoryRepository,
            ISubjectCurriculumRepository subjectCurriculumRepository)
        {
            _examRepository = examRepository;
            _matrixRepository = matrixRepository;
            _examCategoryRepository = examCategoryRepository;
            _subjectCurriculumRepository = subjectCurriculumRepository;
        }

        public async Task<Guid> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            if (await _matrixRepository.GetByIdAsync(request.MatrixId) == null)
                throw new MatrixNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.CategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _subjectCurriculumRepository.GetByIdAsync(request.SubjectCurriculumId) == null)
                throw new SubjectCurriculumNotFoundException();

            var exam = new Exam
            {
                Title = request.Title,
                MatrixId = request.MatrixId,
                CategoryId = request.CategoryId,
                SubjectCurriculumId = request.SubjectCurriculumId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _examRepository.AddAsync(exam);

            return exam.Id;
        }
    }

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Unit>
    {
        private readonly IExamRepository _examRepository;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;

        public UpdateExamCommandHandler(
            IExamRepository examRepository,
            IMatrixRepository matrixRepository,
            IExamCategoryRepository examCategoryRepository,
            ISubjectCurriculumRepository subjectCurriculumRepository)
        {
            _examRepository = examRepository;
            _matrixRepository = matrixRepository;
            _examCategoryRepository = examCategoryRepository;
            _subjectCurriculumRepository = subjectCurriculumRepository;
        }
        public async Task<Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.ExamId);
            if (exam == null)
                throw new ExamNotFoundException();

            if (await _matrixRepository.GetByIdAsync(request.MatrixId) == null)
                throw new MatrixNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.CategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _subjectCurriculumRepository.GetByIdAsync(request.SubjectCurriculumId) == null)
                throw new SubjectCurriculumNotFoundException();

            exam.Title = request.Title;
            exam.MatrixId = request.MatrixId;
            exam.CategoryId = request.CategoryId;
            exam.SubjectCurriculumId = request.SubjectCurriculumId;
            exam.UpdatedBy = request.UpdatedBy;
            exam.UpdatedAt = DateTime.UtcNow;

            await _examRepository.UpdateAsync(exam);
            return Unit.Value;
        }
    }

    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, Unit>
    {
        private readonly IExamRepository _examRepository;
        public DeleteExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }
        public async Task<Unit> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.ExamId);
            if (exam == null)
                throw new ExamNotFoundException();

            exam.DeletedBy = request.DeletedBy;
            exam.DeletedAt = DateTime.UtcNow;

            await _examRepository.DeleteAsync(exam);
            return Unit.Value;
        }
    }
}
