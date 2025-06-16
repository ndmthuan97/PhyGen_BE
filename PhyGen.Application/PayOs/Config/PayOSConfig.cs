﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Config
{
    public class PayOSConfig
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string ChecksumKey { get; set; }
        public string WebhookUrl { get; set; }
    }
}
