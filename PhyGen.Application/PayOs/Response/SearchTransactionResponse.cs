using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Response
{
    public class SearchTransactionResponse
    {
        public int CoinChange { get; set; }
        public int CoinBefore { get; set; }
        public int CoinAfter { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
    }
}
