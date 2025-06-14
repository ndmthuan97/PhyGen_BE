using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Commands
{
    public class DeleteMatrixCommand : IRequest<Unit>
    {
        public Guid MatrixId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
