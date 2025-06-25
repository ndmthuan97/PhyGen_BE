using MediatR;
using PhyGen.Application.MatrixSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Queries
{
    public class GetMatrixSectionsByMatrixIdQuery : IRequest<List<MatrixSectionResponse>>
    {
        public Guid MatrixId { get; set; }
        public GetMatrixSectionsByMatrixIdQuery(Guid matrixId)
        {
            MatrixId = matrixId;
        }
    }
}
