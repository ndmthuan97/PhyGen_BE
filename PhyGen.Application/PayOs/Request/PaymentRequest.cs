using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Request
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; } // URL trả về sau khi thanh toán
        public string CancelUrl { get; set; } // URL khi hủy thanh toán
    }
}
