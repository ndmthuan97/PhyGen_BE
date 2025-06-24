using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public enum PaymentStatus
    {
        Pending,    // Chờ xử lý
        Completed,  // Đã hoàn thành
        Expired,     // Hết hạn
        Cancelled   // Đã hủy
    }
}
