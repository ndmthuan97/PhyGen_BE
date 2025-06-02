using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Guid? CurriculumId { get; set; }

        public int? OrderNo { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("CurriculumId")]
        public virtual Curriculum? Curriculum { get; set; }

        public virtual ICollection<ChapterUnit> ChapterUnits { get; set; } = new List<ChapterUnit>();
        public virtual ICollection<MatrixDetail> MatrixDetails { get; set; } = new List<MatrixDetail>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<BookDetail> BookDetails { get; set; } = new List<BookDetail>();
    }
}
