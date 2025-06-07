using System;
using System.Collections.Generic;
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

        [Required]
        public Guid SubjectCurriculumId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual Matrix Matrix { get; set; } = null!;

        public virtual ExamCategory Category { get; set; } = null!;

        public virtual SubjectCurriculum SubjectCurriculum { get; set; } = null!;

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    }
}
