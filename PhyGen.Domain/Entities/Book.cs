using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Book : EntityBase<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("SeriesId")]
        public virtual BookSeries Series { get; set; } = null!;

        public virtual ICollection<BookDetail> BookDetails { get; set; } = new List<BookDetail>();
    }
}
