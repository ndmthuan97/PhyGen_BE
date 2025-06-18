using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs.SubjectBooks
{
    public class SubjectBookBySubjectIdSpecParam : BaseSpecParam
    {
        public Guid SubjectId { get; set; }
    }
}
