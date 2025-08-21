using MediatR;
using PhyGen.Application.ContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Queries
{
    public class GetContentItemsByContentFlowIdQuery : IRequest<List<ContentItemResponse>>
    {
        public Guid ContentFlowId { get; set; }
        public GetContentItemsByContentFlowIdQuery(Guid contentFlowId)
        {
            ContentFlowId = contentFlowId;
        }
    }
}
