using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Responses
{
    public class SubjectCurriculumResponse
    {
        public Guid SubjectCurriculumId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid CurriculumId { get; set; }
    }
}
