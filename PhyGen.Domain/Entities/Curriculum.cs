using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Curriculum : EntityBase<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Grade { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
