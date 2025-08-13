using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixSectionDetail : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixSectionId { get; set; }

        [Required]
        public Guid SectionId { get; set; }

        [Required]
        public Guid ContentItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }

        public int Quantity { get; set; }

        public DateTime? DeletedAt { get; set; }


        // --- Navigation Properties ---
        public virtual MatrixSection MatrixSection { get; set; } = null!;
        public virtual Section Section { get; set; } = null!;
        public virtual ContentItem ContentItem { get; set; } = null!;
    }
}
