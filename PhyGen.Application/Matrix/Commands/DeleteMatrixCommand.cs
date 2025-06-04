using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrix.Commands
{
    public class DeleteMatrixCommand : IRequest<Unit>
    {
        public Guid MatrixId { get; set; }
        public string? DeletedBy { get; set; } = null!;
    }
}
