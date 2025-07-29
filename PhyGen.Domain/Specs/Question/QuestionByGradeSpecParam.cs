using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs.Question
{
    public class QuestionByGradeSpecParam : BaseSpecParam
    {
        public int Grade { get; set; }

        public string? CreatedBy { get; set; }

        public string? Search { get; set; }

        public string? Sort { get; set; }
    }
}
