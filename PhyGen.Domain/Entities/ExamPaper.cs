using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamPaper : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        public string? Version { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; } = null!;

        public virtual ICollection<ExamPaperQuestion> ExamPaperQuestions { get; set; } = new List<ExamPaperQuestion>();
    }
}
