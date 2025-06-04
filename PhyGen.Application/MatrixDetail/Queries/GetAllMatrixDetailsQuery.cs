using MediatR;
using PhyGen.Application.MatrixDetail.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Queries
{
    public class GetAllMatrixDetailsQuery : IRequest<List<MatrixDetailResponse>>
    {
    }
}
