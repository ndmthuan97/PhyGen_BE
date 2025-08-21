using MediatR;
using PhyGen.Application.MatrixSectionDetails.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Queries
{
    public class GetMatrixSectionDetailsByMatrixSectionIdQuery : IRequest<List<MatrixSectionDetailResponse>>
    {
        public Guid MatrixSectionId { get; set; }
        public GetMatrixSectionDetailsByMatrixSectionIdQuery(Guid matrixSectionId)
        {
            MatrixSectionId = matrixSectionId;
        }
    }
}
