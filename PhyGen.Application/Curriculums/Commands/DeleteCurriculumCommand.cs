using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Commands
{
    public class DeleteCurriculumCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
