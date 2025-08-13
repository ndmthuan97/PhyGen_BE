using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Transaction : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public long? PaymentlinkID { get; set; }
        public int CoinChange { get; set; }
        public int CoinBefore { get; set; }
        public int CoinAfter { get; set; }
        public string TypeChange { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}
