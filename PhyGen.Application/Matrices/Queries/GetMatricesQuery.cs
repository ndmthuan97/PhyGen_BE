using MediatR;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Queries
{
    public record GetMatricesQuery(MatrixSpecParam MatrixSpecParam) : IRequest<Pagination<MatrixResponse>>;
}
