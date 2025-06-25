using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Commands
{
    public class DeleteMatrixSectionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteMatrixSectionCommand(Guid id)
        {
            Id = id;
        }
    }
}
