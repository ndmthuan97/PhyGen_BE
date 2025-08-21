using MediatR;
using PhyGen.Application.MatrixSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Queries
{
    public class GetMatrixSectionByIdQuery : IRequest<List<MatrixSectionResponse>>
    {
        public Guid Id { get; set; }
        public GetMatrixSectionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
