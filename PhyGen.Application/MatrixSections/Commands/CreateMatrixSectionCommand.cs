using MediatR;
using PhyGen.Application.MatrixSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Commands
{
    public class CreateMatrixSectionCommand : IRequest<MatrixSectionResponse>
    {
        public Guid MatrixId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float? Score { get; set; }
        public string? Description { get; set; }
    }
}
