using MediatR;
using PhyGen.Application.Questions.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
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

        public CreateQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Type = request.Type,
                Level = request.Level,
                ChapterId = request.ChapterId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            question = await _questionRepository.AddAsync(question);
            return question.Id;
        }
    }
}
