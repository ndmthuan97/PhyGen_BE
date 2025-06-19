using MediatR;
using PhyGen.Application.Subjects.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Subjects.Commands
{
    public class CreateSubjectCommand : IRequest<SubjectResponse>
    {
        public string Name { get; set; } = string.Empty;
    }
}
