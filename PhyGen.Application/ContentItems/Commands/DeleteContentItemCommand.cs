using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Commands
{
    public class DeleteContentItemCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
