using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Response
{
    public class AdminWeeklyResponse
    {
        public int LoginThisWeek { get; set; }
        public int LoginLastWeek { get; set; }
        public int TotalUserBeforeNow { get; set; }
        public decimal TotalRevenue { get; set; }
        public double RateRevenue { get; set; }
        public double LoginRateBeforeNow { get; set; }
        public double LoginRateBeforeLastWeek { get; set; }

    }
}
