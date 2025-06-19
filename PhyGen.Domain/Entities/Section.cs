using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Section : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } 

        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Exam Exam { get; set; } = null!;
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
    }
}
