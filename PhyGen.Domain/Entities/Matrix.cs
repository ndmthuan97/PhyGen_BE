using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Matrix : EntityBase<Guid>
    {
        [Required]
        public Guid SubjectId { get; set; }

        [Required]
        public Guid ExamCategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        // --- Navigation Properties ---
        public virtual Subject Subject { get; set; } = null!;
        public virtual ExamCategory ExamCategory { get; set; } = null!;
        public virtual ICollection<MatrixContentItem> MatrixContentItems { get; set; } = new List<MatrixContentItem>();
        public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();
    }
}
