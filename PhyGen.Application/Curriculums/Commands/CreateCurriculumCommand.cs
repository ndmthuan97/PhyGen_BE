using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Commands
{
    public class CreateCurriculumCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string? Grade { get; set; }

        public string? Description { get; set; }

        public string? CreatedBy { get; set; }
    }
}
