using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Commands
{
    public class CreateSubjectCurriculumCommand : IRequest<Guid>
    {
        public Guid SubjectId { get; set; }
        public Guid CurriculumId { get; set; }
    }
}
