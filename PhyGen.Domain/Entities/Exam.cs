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
        public Guid UserId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual ExamCategory Category { get; set; } = null!;
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = new List<ExamVersion>();
    }
}
