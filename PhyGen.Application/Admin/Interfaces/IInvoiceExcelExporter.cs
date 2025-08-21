using PhyGen.Application.Admin.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Admin.Interfaces
{
    public interface IInvoiceExcelExporter
    {
        Task<byte[]> ExportAsync(InvoiceExportFilter filter, CancellationToken ct = default);
    }
}
