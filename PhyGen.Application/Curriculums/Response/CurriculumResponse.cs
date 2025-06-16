using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Response
{
    public class CurriculumResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;
    }
}
