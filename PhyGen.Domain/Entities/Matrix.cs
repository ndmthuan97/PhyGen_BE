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
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;

        [Required]
        public Guid SubjectId { get; set; }

        [Required]
        public Guid ExamCategoryId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public virtual Subject Subject { get; set; } = null!;

        public virtual ExamCategory ExamCategory { get; set; } = null!;

        public virtual ICollection<MatrixContentItem> MatrixContentItems { get; set; } = new List<MatrixContentItem>();
    }
}
