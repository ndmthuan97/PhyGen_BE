using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Commands
{
    public class UpdateMatrixSectionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid MatrixId { get; set; }
        public string Title { get; set; } = string.Empty;
        public double? Score { get; set; }
        public string? Description { get; set; }
    }
}
