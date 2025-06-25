using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Commands
{
    public class DeleteMatrixSectionDetailCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteMatrixSectionDetailCommand(Guid id)
        {
            Id = id;
        }
    }
}
