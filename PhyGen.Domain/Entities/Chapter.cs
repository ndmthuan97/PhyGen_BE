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
        public Guid SubjectBookId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public string ChapterCode { get; set; } = string.Empty;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual SubjectBook SubjectBook { get; set; } = null!;
        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
