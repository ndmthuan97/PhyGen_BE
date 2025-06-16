using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamQuestion : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public virtual Exam Exam { get; set; } = null!;

        public virtual Question Question { get; set; } = null!;
    }
}
