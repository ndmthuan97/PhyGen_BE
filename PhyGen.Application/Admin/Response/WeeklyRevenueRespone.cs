using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Response
{
    public class WeeklyRevenueResponse
    {
        public List<WeeklyRevenueItem> WeeklyRevenues { get; set; } = new();
    }
}
