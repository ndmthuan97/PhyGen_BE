using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public virtual MatrixSection MatrixSection { get; set; } = null!;
        public virtual Section Section { get; set; } = null!;
    }
}
