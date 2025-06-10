using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ContentItemExamCategory : EntityBase<int>
    {
        [Required]
        public Guid ContentItemId { get; set; }

        [Required]
        public int ExamCategoryId { get; set; }

        public virtual ContentItem ContentItem { get; set; } = null!;
        public virtual ExamCategory ExamCategory { get; set; } = null!;

    }
}
