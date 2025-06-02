using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Commands
{
    public class UpdateExamCommand : IRequest<Unit>
    {
        public Guid ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}
