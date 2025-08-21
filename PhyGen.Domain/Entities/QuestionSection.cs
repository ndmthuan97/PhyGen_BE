using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class QuestionSection : EntityBase<Guid>
    {
        [Required]
        public Guid SectionId { get; set; }
        [Required]
        public Guid QuestionId { get; set; }

        public double? Score { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Section Section { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
