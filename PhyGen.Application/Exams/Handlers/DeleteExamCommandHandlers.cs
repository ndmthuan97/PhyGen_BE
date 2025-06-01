using MediatR;
using PhyGen.Application.Exams.Commands;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class DeleteExamCommandHandlers : IRequestHandler<DeleteExamCommand, Unit>
    {
        private readonly IExamRepository _examRepository;

        public DeleteExamCommandHandlers(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Unit> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.ExamId) ?? throw new KeyNotFoundException("Exam not found.");

            exam.DeletedBy = request.DeletedBy;
            exam.DeletedAt = DateTime.UtcNow;

            await _examRepository.UpdateAsync(exam);

            return Unit.Value;
        }
    }
}
