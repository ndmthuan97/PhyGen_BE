using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Request
{
    public class WebhookResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
