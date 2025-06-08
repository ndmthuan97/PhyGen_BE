using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ChapterUnit : EntityBase<Guid>
    {
        [Required]
        public Guid ChapterId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        public int? OrderNo { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual Chapter Chapter { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
