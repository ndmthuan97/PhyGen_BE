using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new Dictionary<StatusCode, string>
        {
            
        };

        public static string GetMessage(StatusCode code) => _messages[code];
    }
}
