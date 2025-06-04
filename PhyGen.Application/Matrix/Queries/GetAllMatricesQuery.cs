using MediatR;
using PhyGen.Application.Matrix.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrix.Queries
{
    public class GetAllMatricesQuery : IRequest<List<MatrixResponse>>
    { 
    }
}
