using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ContentItem : EntityBase<Guid>
    {
        public Guid ContentFlowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public virtual ContentFlow ContentFlow { get; set; } = null!;
        public virtual ICollection<ContentItemExamCategory> ContentItemExamCategories { get; set; } = new List<ContentItemExamCategory>();

    }
}
