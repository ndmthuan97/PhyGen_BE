using MediatR;
using PhyGen.Application.ContentFlows.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Queries
{
    public class GetAllContentFlowsQuery : IRequest<List<ContentFlowResponse>>
    {
    }
}
