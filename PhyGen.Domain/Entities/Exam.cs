using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Exam : EntityBase<Guid>
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("MatrixId")]
        public virtual Matrix Matrix { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public virtual ExamCategory Category { get; set; } = null!;

        public virtual ICollection<ExamPaper> ExamPapers { get; set; } = new List<ExamPaper>();
    }
}
