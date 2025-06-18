using MediatR;
using PhyGen.Application.Curriculums.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Commands
{
    public class CreateCurriculumCommand : IRequest<CurriculumResponse>
    {
        public string Name { get; set; } = string.Empty;

        public int Grade { get; set; }
    }
}
