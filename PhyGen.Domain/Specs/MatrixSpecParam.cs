using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    public class MatrixSpecParam : BaseSpecParam
    {
        public Guid? SubjectId { get; set; }

        public Guid? ExamCategoryId { get; set; }

        public int? Grade { get; set; }

        public int? Year { get; set; }

        public string? Search { get; set; }

        public string? Sort { get; set; }
    }
}
