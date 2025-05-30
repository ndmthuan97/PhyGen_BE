using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Commands
{
    public class UpdateCurriculumCommand : IRequest<Unit>
    {
        public Guid CurriculumId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Grade { get; set; }

        public string? Description { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
