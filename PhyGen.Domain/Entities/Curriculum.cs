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
    }
}
