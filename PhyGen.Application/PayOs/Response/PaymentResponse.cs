using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Response
{
    public class PaymentResponse
    {
        public string CheckoutUrl { get; set; }
        public long PaymentLinkId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
