using MediatR;
using PhyGen.Application.MatrixContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Commands
{
    public class CreateMatrixContentItemCommand : IRequest<MatrixContentItemResponse>
    {
        public Guid MatrixId { get; set; }
        public Guid ContentItemId { get; set; }
    }
}
