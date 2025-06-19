using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamVersion : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        [Required]
        public string Code { get; set; } = string.Empty;

        public string QuestionOrder { get; set; } = string.Empty;

        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Exam Exam { get; set; } = null!;
    }
}
