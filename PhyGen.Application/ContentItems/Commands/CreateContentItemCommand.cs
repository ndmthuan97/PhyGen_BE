using MediatR;
using PhyGen.Application.ContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Commands
{
    public class CreateContentItemCommand : IRequest<ContentItemResponse>
    {
        public Guid ContentFlowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
}
