using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Commands
{
    public class CreateContentItemCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }
}
