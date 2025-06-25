using MediatR;
using PhyGen.Application.MatrixSectionDetails.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Queries
{
    public class GetMatrixSectionDetailByIdQuery : IRequest<MatrixSectionDetailResponse>
    {
        public Guid Id { get; set; }
        public GetMatrixSectionDetailByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
