using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Topic : EntityBase<Guid>
    {
        [Required]
        public Guid ChapterId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int? OrderNo { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual Chapter Chapter { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
