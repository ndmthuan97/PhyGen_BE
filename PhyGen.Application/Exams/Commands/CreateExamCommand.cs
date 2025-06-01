using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Commands
{
    public class CreateExamCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public Guid MatrixId { get; set; }
        public int CategoryId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
