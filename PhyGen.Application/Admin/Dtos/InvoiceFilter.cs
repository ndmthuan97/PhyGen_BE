using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Dtos
{
    public class InvoiceFilter : BaseSpecParam
    {
        public string? FullName { get; set; }
        public string? Status { get; set; }
        public decimal? MinAmount { get; set; } // số xu tối thiểu
    }
}
