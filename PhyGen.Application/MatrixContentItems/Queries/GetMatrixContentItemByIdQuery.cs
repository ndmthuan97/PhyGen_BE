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
        public Guid Id { get; set; }
        public GetMatrixContentItemByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
