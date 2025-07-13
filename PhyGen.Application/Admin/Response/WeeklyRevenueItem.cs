using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Response
{
    public class WeeklyRevenueItem
    {
        public string WeekRange { get; set; } = string.Empty; 
        public decimal Revenue { get; set; }
    }
}
