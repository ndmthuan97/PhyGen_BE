using PhyGen.Application.Admin.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Interfaces
{
    public interface IStatisticService
    {
        Task<InvoiceResponse> GetInvoiceStatistics();
        Task<AdminWeeklyResponse> GetWeeklyStatisticsAsync();
        Task<WeeklyRevenueResponse> GetAllWeeklyRevenueAsync();
    }
}
