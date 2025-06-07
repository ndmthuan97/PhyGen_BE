using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class EmailOtpManager
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string Otptext { get; set; } = null!;
        public string? Otptype { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime? Createddate { get; set; }
    }
}
