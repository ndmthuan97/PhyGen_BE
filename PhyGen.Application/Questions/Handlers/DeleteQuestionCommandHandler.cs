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
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId) 
                ?? throw new KeyNotFoundException("Question not found.");

            question.DeletedBy = request.DeletedBy;
            question.DeletedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }
}
