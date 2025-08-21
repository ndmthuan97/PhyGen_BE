using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Payment : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public long PaymentLinkId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
