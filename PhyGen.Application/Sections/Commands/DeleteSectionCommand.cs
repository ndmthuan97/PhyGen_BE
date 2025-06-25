using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Commands
{
    public class DeleteSectionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteSectionCommand(Guid id)
        {
            Id = id;
        }
    }
}
