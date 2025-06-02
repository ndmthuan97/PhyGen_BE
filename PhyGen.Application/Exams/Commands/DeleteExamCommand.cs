using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Commands
{
    public class DeleteExamCommand : IRequest<Unit>
    {
        public Guid ExamId { get; set; }
        public string? DeletedBy { get; set; } = null!;
    }
}
