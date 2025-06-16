using MediatR;
using PhyGen.Application.Matrices.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Queries
{
    public class GetAllMatricesQuery : IRequest<List<MatrixResponse>>
    {
    }
}
