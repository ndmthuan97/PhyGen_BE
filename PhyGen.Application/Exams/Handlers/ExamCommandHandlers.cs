using MediatR;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, ExamResponse>
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateExamCommandHandler(IExamRepository examRepository, IExamCategoryRepository examCategoryRepository)
        {
            _examRepository = examRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<ExamResponse> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var category = await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId);
            if (category == null || category.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            if (request.Year < 2018 || request.Year > DateTime.Now.Year)
                throw new Exception("Năm phải nằm trong khoảng từ 2018 đến hiện tại.");

            var exam = new Exam
            {
                ExamCategoryId = request.ExamCategoryId,
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
                Grade = request.Grade,
                Year = request.Year,
                TotalQuestionCount = request.TotalQuestionCount,
                VersionCount = request.VersionCount,
                RandomizeQuestions = request.RandomizeQuestions,
                ImgUrl = request.ImgUrl,
                Status = request.Status,
                ExamCode = await _examRepository.GenerateCodeAsync<Exam>("E", e => e.ExamCode),
            };

            await _examRepository.AddAsync(exam);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamResponse>(exam);
        }
    }

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Unit>
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public UpdateExamCommandHandler(IExamRepository examRepository, IExamCategoryRepository examCategoryRepository)
        {
            _examRepository = examRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.Id);
            if (exam == null)
                throw new ExamNotFoundException();

            var category = await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId);
            if (category == null || category.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            if (request.Year < 2018 || request.Year > DateTime.Now.Year)
                throw new Exception("Năm phải nằm trong khoảng từ 2018 đến hiện tại.");

            exam.ExamCategoryId = request.ExamCategoryId;
            exam.Title = request.Title;
            exam.Description = request.Description;
            exam.Grade = request.Grade;
            exam.Year = request.Year;
            exam.TotalQuestionCount = request.TotalQuestionCount;
            exam.VersionCount = request.VersionCount;
            exam.RandomizeQuestions = request.RandomizeQuestions;
            exam.ImgUrl = request.ImgUrl;
            exam.Status = request.Status;

            await _examRepository.UpdateAsync(exam);
            return Unit.Value;
        }
    }

    public class UpdateExamStatusCommandHandler : IRequestHandler<UpdateExamStatusCommand, Unit>
    {
        private readonly IExamRepository _examRepository;

        public UpdateExamStatusCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Unit> Handle(UpdateExamStatusCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                var exam = await _examRepository.GetByIdAsync(id);

                if (exam == null)
                    throw new ExamNotFoundException();

                exam.Status = request.Status;

                await _examRepository.UpdateAsync(exam);
            }

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
            var exam = await _examRepository.GetByIdAsync(request.Id);
            if (exam == null || exam.DeletedAt.HasValue)
                throw new ExamNotFoundException();

            exam.DeletedAt = DateTime.UtcNow;
            exam.Status = StatusQEM.Removed;

            await _examRepository.UpdateAsync(exam);
            return Unit.Value;
        }
    }
}
