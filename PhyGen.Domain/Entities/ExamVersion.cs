using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamVersion : EntityBase<Guid>
    {
        public Guid ExamId { get; set; }
    }
}
