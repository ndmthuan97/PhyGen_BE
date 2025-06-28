using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamCategory : EntityBase<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Matrix> Matrices { get; set; } = new List<Matrix>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<ExamCategoryChapter> ExamCategoryChapters { get; set; } = new List<ExamCategoryChapter>();
        public virtual ICollection<ContentItemExamCategory> ContentItemExamCategories { get; set; } = new List<ContentItemExamCategory>();
    }
}
