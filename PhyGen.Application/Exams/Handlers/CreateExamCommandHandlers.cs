using MediatR;
using PhyGen.Application.Exams.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class CreateExamCommandHandlers : IRequestHandler<CreateExamCommand, Guid>
    {
        private readonly IExamRepository _examRepository;
        public CreateExamCommandHandlers(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Guid> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = new Exam
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                MatrixId = request.MatrixId,
                CategoryId = request.CategoryId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            exam = await _examRepository.AddAsync(exam);
            return exam.Id;
        }
    }
}
