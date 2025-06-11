using MediatR;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Guid>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IChapterUnitRepository _chapterUnitRepository;

        public CreateQuestionCommandHandler(IQuestionRepository questionRepository, IChapterUnitRepository chapterUnitRepository)
        {
            _questionRepository = questionRepository;
            _chapterUnitRepository = chapterUnitRepository;
        }

        public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                Content = request.Content,
                Type = request.Type,
                Level = request.Level,
                Image = request.Image,
                ChapterUnitId = request.ChapterUnitId,
                Answer1 = request.Answer1,
                Answer2 = request.Answer2,
                Answer3 = request.Answer3,
                Answer4 = request.Answer4,
                Answer5 = request.Answer5,
                Answer6 = request.Answer6,
                CorrectAnswer = request.CorrectAnswer,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };

            await _questionRepository.AddAsync(question);
            return question.Id;
        }
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IChapterUnitRepository _chapterUnitRepository;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IChapterUnitRepository chapterUnitRepository)
        {
            _questionRepository = questionRepository;
            _chapterUnitRepository = chapterUnitRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                Content = request.Content,
                Type = request.Type,
                Level = request.Level,
                Image = request.Image,
                ChapterUnitId = request.ChapterUnitId,
                Answer1 = request.Answer1,
                Answer2 = request.Answer2,
                Answer3 = request.Answer3,
                Answer4 = request.Answer4,
                Answer5 = request.Answer5,
                Answer6 = request.Answer6,
                CorrectAnswer = request.CorrectAnswer,
                UpdatedBy = request.UpdatedBy,
                UpdatedAt = DateTime.UtcNow,
            };

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId);
            if (question == null)
                throw new QuestionNotFoundException();

            question.DeletedBy = request.DeletedBy;
            question.DeletedAt = DateTime.UtcNow;

            await _questionRepository.DeleteAsync(question);
            return Unit.Value;
        }
    }
}
