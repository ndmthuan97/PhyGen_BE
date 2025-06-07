using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ContentFlow : EntityBase<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<ContentItem> ContentItems { get; set; } = new List<ContentItem>();

    }
}
