using MediatR;
using PhyGen.Application.ContentFlows.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Commands
{
    public class CreateContentFlowCommand : IRequest<ContentFlowResponse>
    {
        public Guid CurriculumId { get; set; }
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
}
