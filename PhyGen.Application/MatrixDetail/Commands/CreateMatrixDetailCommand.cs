using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Commands
{
    public class CreateMatrixDetailCommand : IRequest<Guid>
    {
        public Guid MatrixId { get; set; }
        public Guid ChapterId { get; set; }
        public string? Level { get; set; }
        public int? Quantity { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
    }
}
