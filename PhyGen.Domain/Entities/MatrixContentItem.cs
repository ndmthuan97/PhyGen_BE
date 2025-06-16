using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixContentItem : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        public Guid ContentItemId { get; set; }

        public virtual Matrix Matrix { get; set; } = null!;
        public virtual ContentItem ContentItem { get; set; } = null!;

    }
}
