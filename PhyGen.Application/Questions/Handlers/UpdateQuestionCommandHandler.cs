using MediatR;
using PhyGen.Application.Questions.Commands;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException("Question not found.");
            }

            question.Content = request.Content;
            question.UpdatedBy = request.UpdatedBy.ToString();
            question.UpdatedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }
}
