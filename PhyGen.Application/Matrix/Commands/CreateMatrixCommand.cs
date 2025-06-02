using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrix.Commands
{
    public class CreateMatrixCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Grade { get; set; }
        public Guid UserId { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
    }
}
