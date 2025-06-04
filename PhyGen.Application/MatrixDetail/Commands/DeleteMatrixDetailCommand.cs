using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Commands
{
    public class DeleteMatrixDetailCommand : IRequest<Unit>
    {
        public Guid MatrixDetailId { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
    }
}
