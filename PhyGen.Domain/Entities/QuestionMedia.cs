using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class QuestionMedia : EntityBase<Guid>
    {
        [Required]
        public Guid QuestionId { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public virtual Question Question { get; set; } = null!;
    }
}
