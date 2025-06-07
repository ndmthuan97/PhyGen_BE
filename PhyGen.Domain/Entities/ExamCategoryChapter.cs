using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamCategoryChapter : EntityBase<int>
    {
        [Required]
        public Guid ExamCategoryId { get; set; }

        [Required]
        public Guid ChapterId { get; set; }

        public virtual ExamCategory ExamCategory { get; set; } = null!;
        public virtual Chapter Chapter { get; set; } = null!;

    }
}
