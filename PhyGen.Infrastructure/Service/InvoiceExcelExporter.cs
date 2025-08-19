using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Admin.Dtos;
using PhyGen.Application.Admin.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;

public sealed class InvoiceExcelExporter : IInvoiceExcelExporter
{
    private readonly AppDbContext _context;
    public InvoiceExcelExporter(AppDbContext context) => _context = context;

    public async Task<byte[]> ExportAsync(InvoiceExportFilter filter, CancellationToken ct = default)
    {
        var q = from p in _context.Payments.AsNoTracking()
                join u in _context.Users.AsNoTracking() on p.UserId equals u.Id
                select new
                {
                    p.PaymentLinkId,
                    p.CreatedAt,
                    p.Amount,       
                    p.Status,
                    u.FirstName,
                    u.LastName,
                    u.photoURL
                };

        if (!string.IsNullOrWhiteSpace(filter.FullName))
        {
            var terms = filter.FullName.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var t in terms)
            {
                var like = $"%{t}%";
                q = q.Where(x =>
                    EF.Functions.ILike(((x.FirstName ?? "") + " " + (x.LastName ?? "")), like) ||
                    EF.Functions.ILike(x.FirstName ?? "", like) ||
                    EF.Functions.ILike(x.LastName ?? "", like)
                );
            }
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var status = filter.Status.Trim();
            q = q.Where(x => x.Status == status);
        }

        if (filter.MinAmount.HasValue && filter.MinAmount.Value > 0)
        {
            var min = filter.MinAmount.Value / 1000m;
            q = q.Where(x => x.Amount >= min);
        }

        if (filter.FromDate.HasValue && filter.ToDate.HasValue)
        {
            var from = filter.FromDate.Value.Date;
            var to = filter.ToDate.Value.Date.AddDays(1).AddTicks(-1);
            q = q.Where(x => x.CreatedAt >= from && x.CreatedAt <= to);
        }

        var rows = await q
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                InvoiceId = x.PaymentLinkId.ToString(),
                FullName = ((x.FirstName ?? "") + " " + (x.LastName ?? "")).Trim(),
                CreatedAt = x.CreatedAt,
                AmountVnd = x.Amount * 1000m,
                PaymentMethod = "PayOS",
                Status = x.Status
            })
            .ToListAsync(ct);

        var totalBill = await _context.Payments.AsNoTracking().CountAsync(ct);
        var pending = await _context.Payments.AsNoTracking().CountAsync(p => p.Status == PaymentStatus.Pending.ToString(), ct);
        var completed = await _context.Payments.AsNoTracking().CountAsync(p => p.Status == PaymentStatus.Completed.ToString(), ct);
        var canceled = await _context.Payments.AsNoTracking().CountAsync(p =>
                              p.Status == PaymentStatus.Cancelled.ToString() || p.Status == PaymentStatus.Expired.ToString(), ct);

        using var wb = new XLWorkbook();

        // --- Sheet 1: Danh sách hoá đơn ---
        var ws = wb.Worksheets.Add("Invoices");

        // Tiêu đề
        ws.Cell("A1").Value = "BÁO CÁO HOÁ ĐƠN / DOANH THU";
        ws.Range("A1:G1").Merge();
        ws.Range("A1:G1").Style.Font.SetBold().Font.SetFontSize(16)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        // Khoảng thời gian
        var rangeText = (filter.FromDate, filter.ToDate) switch
        {
            (null, null) => "Tất cả thời gian",
            (var f, null) => $"Từ {f:dd/MM/yyyy}",
            (null, var t) => $"Đến {t:dd/MM/yyyy}",
            (var f, var t) => $"Từ {f:dd/MM/yyyy} đến {t:dd/MM/yyyy}"
        };
        ws.Cell("A2").Value = rangeText;
        ws.Range("A2:G2").Merge()
            .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        // Header
        var headerRow = 4;
        var header = new[] { "STT", "Mã hoá đơn", "Họ tên", "Ngày tạo", "Số tiền (₫)", "Phương thức", "Trạng thái" };
        for (int i = 0; i < header.Length; i++)
            ws.Cell(headerRow, i + 1).Value = header[i];

        ws.Range(headerRow, 1, headerRow, header.Length).Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.LightGray)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            .Border.SetInsideBorder(XLBorderStyleValues.Thin);

        // Body
        var r = headerRow + 1;
        var idx = 1;
        foreach (var x in rows)
        {
            ws.Cell(r, 1).Value = idx++;
            ws.Cell(r, 2).Value = x.InvoiceId;
            ws.Cell(r, 3).Value = x.FullName;
            ws.Cell(r, 4).Value = x.CreatedAt;
            ws.Cell(r, 4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            ws.Cell(r, 5).Value = x.AmountVnd;
            ws.Cell(r, 5).Style.NumberFormat.Format = "#,##0";
            ws.Cell(r, 6).Value = x.PaymentMethod;
            ws.Cell(r, 7).Value = x.Status;
            r++;
        }

        // Tổng dòng
        ws.Cell(r, 1).Value = "TỔNG";
        ws.Range(r, 1, r, 4).Merge().Style.Font.SetBold();
        ws.Cell(r, 5).FormulaA1 = $"SUM(E{headerRow + 1}:E{r - 1})";
        ws.Cell(r, 5).Style.NumberFormat.Format = "#,##0";
        ws.Range(headerRow, 1, r, header.Length).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        ws.Range(headerRow, 1, r, header.Length).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Freeze & Auto
        ws.SheetView.FreezeRows(4);
        ws.Range(headerRow, 1, r - 1, header.Length).SetAutoFilter();
        ws.Columns().AdjustToContents();
        ws.Column(3).Width = Math.Max(ws.Column(3).Width, 22); // FullName dễ dài

        // --- Sheet 2: Tổng quan ---
        var s2 = wb.Worksheets.Add("Tổng quan");
        s2.Cell("A1").Value = "TỔNG QUAN HOÁ ĐƠN";
        s2.Range("A1:B1").Merge().Style
            .Font.SetBold().Font.SetFontSize(14)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        s2.Cell("A3").Value = "Tổng hoá đơn trong hệ thống";
        s2.Cell("B3").Value = totalBill;

        s2.Cell("A4").Value = "Đang chờ (Pending)";
        s2.Cell("B4").Value = pending;

        s2.Cell("A5").Value = "Hoàn tất (Completed)";
        s2.Cell("B5").Value = completed;

        s2.Cell("A6").Value = "Huỷ/Expired";
        s2.Cell("B6").Value = canceled;

        s2.Cell("A8").Value = "Tổng số tiền trong kết quả lọc";
        s2.Cell("B8").FormulaA1 = $"'{ws.Name}'!E{r}";   // link sang dòng tổng sheet 1
        s2.Cell("B8").Style.NumberFormat.Format = "#,##0";

        s2.Columns().AdjustToContents();

        // 3) Xuất ra byte[]
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
