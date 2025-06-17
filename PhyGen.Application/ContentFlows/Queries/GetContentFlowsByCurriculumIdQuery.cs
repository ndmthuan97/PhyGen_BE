using MediatR;
using PhyGen.Application.ContentFlows.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Queries
{
    public class GetContentFlowsByCurriculumIdQuery : IRequest<List<ContentFlowResponse>>
    {
        public Guid CurriculumId { get; set; }
        public GetContentFlowsByCurriculumIdQuery(Guid curriculumId)
        {
            CurriculumId = curriculumId;
        }
    }
}
