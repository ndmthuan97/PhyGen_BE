using MediatR;
using PhyGen.Application.MatrixContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Queries
{
    public class GetMatrixContentItemByIdQuery : IRequest<MatrixContentItemResponse>
    {
        public int MatrixContetnItemId { get; set; }
        public GetMatrixContentItemByIdQuery(int id)
        {
            MatrixContetnItemId = id;
        }
    }
}
