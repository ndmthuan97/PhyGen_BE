using MediatR;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Queries
{
    public record GetMatrixSectionsByMatrixIdQuery(MatrixSectionSpecParam param) : IRequest<Pagination<MatrixSectionResponse>>;
}
