using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Question : EntityBase<Guid>
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public string? Type { get; set; }

        public string? Level { get; set; }

        public string? Image { get; set; }

        [Required]
        public Guid ChapterId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ChapterId")]
        public virtual Chapter Chapter { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; } = null!;

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<ExamPaperQuestion> ExamPaperQuestions { get; set; } = new List<ExamPaperQuestion>();
    }
}
