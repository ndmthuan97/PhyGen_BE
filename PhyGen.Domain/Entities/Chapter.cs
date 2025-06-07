using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Chapter : EntityBase<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid SubjectCurriculumId { get; set; }

        public int? OrderNo { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual SubjectCurriculum SubjectCurriculum { get; set; } = null!;
        public virtual ICollection<ChapterUnit> ChapterUnits { get; set; } = new List<ChapterUnit>();
    }
}
