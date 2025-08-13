using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Question : EntityBase<Guid>
    {
        public Guid? TopicId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public QuestionType Type { get; set; }
        public DifficultyLevel Level { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }

        public int Grade { get; set; }

        public StatusQEM Status { get; set; } = StatusQEM.Draft;

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Topic Topic { get; set; } = null!;
        public virtual ICollection<QuestionMedia> QuestionMedias { get; set; } = new List<QuestionMedia>();
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
    }
}
