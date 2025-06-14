using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Commands
{
    public class CreateMatrixContentItemCommand : IRequest<int>
    {
        public Guid MatrixId { get; set; }
        public Guid ContentItemId { get; set; }
    }
}
