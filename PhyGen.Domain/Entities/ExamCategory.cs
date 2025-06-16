using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamCategory : EntityBase<int>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<Matrix> Matrices { get; set; } = new List<Matrix>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<ExamCategoryChapter> ExamCategoryChapters { get; set; } = new List<ExamCategoryChapter>();

    }
}
