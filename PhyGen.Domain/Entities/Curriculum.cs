using System;
using System.Collections.Generic;
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
        public int Year { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<ContentFlow> ContentFlows { get; set; } = new List<ContentFlow>();
    }
}
