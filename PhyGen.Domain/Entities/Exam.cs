using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Exam : EntityBase<Guid>
    {
        [Required]
        public Guid ExamCategoryId { get; set; }
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public int Grade { get; set; }
        public int Year { get; set; }
        public int? TotalQuestionCount { get; set; }

        public int VersionCount { get; set; }
        public bool RandomizeQuestions { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual ExamCategory ExamCategory { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = new List<ExamVersion>();
    }
}
