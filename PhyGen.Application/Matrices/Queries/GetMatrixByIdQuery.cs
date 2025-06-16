using MediatR;
using PhyGen.Application.Matrices.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Queries
{
    public class GetMatrixByIdQuery : IRequest<MatrixResponse>
    {
        public Guid MatrixId { get; set; }
        public GetMatrixByIdQuery(Guid id)
        {
            MatrixId = id;
        }
    }
}
