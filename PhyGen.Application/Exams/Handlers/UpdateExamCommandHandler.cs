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
    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Unit>
    {
        private readonly IExamRepository _examRepository;

        public UpdateExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task <Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.ExamId);
            if (exam == null)
            {
                throw new KeyNotFoundException("Exam not found.");
            }

            exam.Title = request.Title;
            exam.UpdatedBy = request.UpdatedBy;
            exam.UpdatedAt = DateTime.UtcNow;

            await _examRepository.UpdateAsync(exam);

            return Unit.Value;
        }
    }
}
