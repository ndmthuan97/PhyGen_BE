using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public abstract class EntityBase<T>
    {
        [Key]
        public T Id { get; set; } = default!;
    }
}
