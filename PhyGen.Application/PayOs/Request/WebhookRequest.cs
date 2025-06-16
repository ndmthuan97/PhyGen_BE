using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Request
{
    public class WebhookRequest
    {
        public string TransactionId { get; set; } = null!;
    }
}
