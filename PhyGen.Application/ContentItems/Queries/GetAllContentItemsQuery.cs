using MediatR;
using PhyGen.Application.ContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Queries
{
    public class GetAllContentItemsQuery : IRequest<List<ContentItemResponse>>
    {
    }
}
