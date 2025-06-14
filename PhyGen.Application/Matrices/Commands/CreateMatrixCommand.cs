using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Commands
{
    public class CreateMatrixCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
    }
}
