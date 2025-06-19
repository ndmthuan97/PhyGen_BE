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
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        public string? Image { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }

        public string? CorrectAnswer { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Topic Topic { get; set; } = null!;
        public virtual ICollection<QuestionMedia> QuestionMedias { get; set; } = new List<QuestionMedia>();
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
    }
}
