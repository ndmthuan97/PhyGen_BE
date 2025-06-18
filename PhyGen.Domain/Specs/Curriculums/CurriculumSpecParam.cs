using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs.Curriculums
{
    public class CurriculumSpecParam : BaseSpecParam
    {
        public string? Search { get; set; }

        public string Sort { get; set; } = "Name";
    }
}
