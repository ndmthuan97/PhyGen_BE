using MediatR;
using PhyGen.Application.MatrixContentItems.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Queries
{
    public class GetMatrixContentItemsByMatrixIdAndContentItemIdQuery : IRequest<MatrixContentItemResponse>
    {
        public Guid MatrixId { get; set; }
        public Guid ContentItemId { get; set; }
        public GetMatrixContentItemsByMatrixIdAndContentItemIdQuery(Guid matrixId, Guid contentItemId)
        {
            MatrixId = matrixId;
            ContentItemId = contentItemId;
        }
    }
}
