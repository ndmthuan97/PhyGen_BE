using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixDetail : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        public Guid ChapterId { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public string? CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("MatrixId")]
        public virtual Matrix Matrix { get; set; } = null!;

        [ForeignKey("ChapterId")]
        public virtual Chapter Chapter { get; set; } = null!;
    }
}
