using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixSection : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public double? Score { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Matrix Matrix { get; set; } = null!;
        public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();
    }

}
