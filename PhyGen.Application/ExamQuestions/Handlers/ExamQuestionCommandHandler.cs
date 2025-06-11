using MediatR;
using PhyGen.Application.ExamQuestions.Commands;
using PhyGen.Application.ExamQuestions.Exceptions;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Handlers
{
    public class CreateExamQuestionCommandHandler : IRequestHandler<CreateExamQuestionCommand, Guid>
    {
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;

        public CreateExamQuestionCommandHandler(
            IExamQuestionRepository examQuestionRepository,
            IExamRepository examRepository,
            IQuestionRepository questionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Guid> Handle(CreateExamQuestionCommand request, CancellationToken cancellationToken)
        {
            if (await _examRepository.GetByIdAsync(request.ExamId) == null)
                throw new ExamNotFoundException();

            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
                throw new QuestionNotFoundException();

            var examQuestion = new ExamQuestion
            {
                ExamId = request.ExamId,
                QuestionId = request.QuestionId
            };

            await _examQuestionRepository.AddAsync(examQuestion);
            return examQuestion.Id;
        }
    }

    public class UpdateExamQuestionCommandHandler : IRequestHandler<UpdateExamQuestionCommand, Unit>
    {
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;

        public UpdateExamQuestionCommandHandler(
            IExamQuestionRepository examQuestionRepository,
            IExamRepository examRepository,
            IQuestionRepository questionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(UpdateExamQuestionCommand request, CancellationToken cancellationToken)
        {
            if (await _examQuestionRepository.GetByIdAsync(request.ExamQuestionId) == null)
                throw new ExamQuestionNotFoundException();

            if (await _examRepository.GetByIdAsync(request.ExamId) == null)
                throw new ExamNotFoundException();

            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
                throw new QuestionNotFoundException();

            var examQuestion = new ExamQuestion
            {
                ExamId = request.ExamId,
                QuestionId = request.QuestionId
            };

            await _examQuestionRepository.UpdateAsync(examQuestion);
            return Unit.Value;
        }
    }
}
