using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class BookDetail : EntityBase<Guid>
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        public Guid ChapterId { get; set; }

        public int? OrderNo { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey(nameof(ChapterId))]
        public virtual Chapter Chapter { get; set; } = null!;
    }

}
