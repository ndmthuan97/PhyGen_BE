using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class SubjectBook : EntityBase<Guid>
    {
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
