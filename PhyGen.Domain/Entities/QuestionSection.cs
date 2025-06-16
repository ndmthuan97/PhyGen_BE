using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class QuestionSection : EntityBase<Guid>
    {
        public Guid QuestionId { get; set; }
        public Guid SectionId { get; set; }
        // Navigation Properties
        public virtual Question Question { get; set; } = null!;
        public virtual Section Section { get; set; } = null!;
    }
}
