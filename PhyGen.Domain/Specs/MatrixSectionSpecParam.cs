using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    public class MatrixSectionSpecParam : BaseSpecParam
    {
        public Guid? MatrixId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }

    }
}
