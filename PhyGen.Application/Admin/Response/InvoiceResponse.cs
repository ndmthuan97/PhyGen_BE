using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Response
{
    public class InvoiceResponse
    {
        public int TotalBill { get; set; }
        public int PendingBill { get; set; }
        public int CompletedBill { get; set; }
        public int CanceledBill { get; set; }
        public Pagination<InvoiceItem> Invoices { get; set; }

    }
    public class InvoiceItem
    {
        public string InvoiceId { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "PayOS";
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
    }
}
