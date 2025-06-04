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

        public string? Description { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
