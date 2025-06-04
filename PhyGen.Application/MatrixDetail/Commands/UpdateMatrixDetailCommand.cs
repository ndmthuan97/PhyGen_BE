using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Commands
{
    public class UpdateMatrixDetailCommand : IRequest<Unit>
    {
        public Guid MatrixDetailId { get; set; }
        public Guid MatrixId { get; set; }
        public Guid ChapterId { get; set; }
        public string? Level { get; set; }
        public int? Quantity { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
    }
}
