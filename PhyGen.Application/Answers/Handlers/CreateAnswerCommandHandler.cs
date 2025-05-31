using MediatR;
using PhyGen.Application.Answers.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Handlers
{
    public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, Guid>
    {
        private readonly IAnswerRepository _answerRepository;

        public CreateAnswerCommandHandler(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<Guid> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = new Answer
            {
                Content = request.Content,
                IsCorrect = request.IsCorrect,
                QuestionId = request.QuestionId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            answer = await _answerRepository.AddAsync(answer);
            return answer.Id;
        }
    }
}
