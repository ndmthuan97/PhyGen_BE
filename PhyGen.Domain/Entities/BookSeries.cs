using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class BookSeries : EntityBase<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
