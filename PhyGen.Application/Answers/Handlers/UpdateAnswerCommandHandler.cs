using MediatR;
using PhyGen.Application.Answers.Commands;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Handlers
{
    public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, Unit>
    {
        private readonly IAnswerRepository _answerRepository;

        public UpdateAnswerCommandHandler(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<Unit> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await _answerRepository.GetByIdAsync(request.Id);
            if (answer == null) throw new Exception("Answer not found");

            answer.Content = request.Content;
            answer.IsCorrect = request.IsCorrect;
            answer.UpdatedBy = request.UpdatedBy;
            answer.UpdatedAt = DateTime.UtcNow;

            await _answerRepository.UpdateAsync(answer);
            return Unit.Value;
        }
    }
}
