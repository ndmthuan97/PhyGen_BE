using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Admin.Dtos;
using PhyGen.Application.Admin.Interfaces;
using PhyGen.Application.Admin.Response;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PhyGen.Infrastructure.Service
{
    public class AdminService : IStatisticService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminWeeklyResponse> GetWeeklyStatisticsAsync()
        {
            var now = DateTime.UtcNow;

            var startOfThisWeek = now.Date.AddDays(-(int)now.DayOfWeek);       // Chủ nhật đầu tuần
            var startOfLastWeek = startOfThisWeek.AddDays(-7);
            var endOfLastWeek = startOfThisWeek.AddSeconds(-1);

            // Người login
            var loginThisWeek = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin >= startOfThisWeek);

            var loginLastWeek = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin >= startOfLastWeek && u.LastLogin <= endOfLastWeek);

            // Doanh thu
            var totalRevenue = await _context.Payments
                .Where(p => p.CreatedAt <= now && p.Status == "Completed")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var TotalRevenueLastWeek = await _context.Payments
                .Where(p => p.CreatedAt <= startOfLastWeek && p.Status == "Completed")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Tổng người dùng trước tuần này
            var totalUserBeforeLastWeek = await _context.Users
                .CountAsync(u => u.CreatedAt < startOfLastWeek);

            // Tổng login trước tuần trước
            var totalLoginBeforeLastWeek = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin < startOfLastWeek);

            var totalBook = await _context.SubjectBooks.CountAsync();

            var totalQuestion = await _context.Questions
                .Where(q => q.DeletedAt == null && q.CreatedBy == "Admin")
                .CountAsync();

            // Tổng question trước tuần trước
            var totalQuestionLastWeek = await _context.Questions
                .Where(q => q.DeletedAt == null && q.CreatedBy == "Admin" && q.CreatedAt < startOfLastWeek)
                .CountAsync();
            // Tổng trước thời điểm hiện tại
            var totalUserBeforeNow = await _context.Users
                .CountAsync(u => u.CreatedAt < now);

            var totalLoginBeforeNow = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin < now);

            double revenueRate = (double)(TotalRevenueLastWeek > 0
                ? Math.Round((totalRevenue - TotalRevenueLastWeek) / totalRevenue * 100, 2)
                : (totalRevenue > 0 ? 100 : 0));
            // Tính tỷ lệ login
            double loginRateBeforeNow = totalUserBeforeNow > 0
                ? Math.Round((double)totalLoginBeforeNow / totalUserBeforeNow * 100, 2)
                : 0;
            double userRateNow = totalUserBeforeLastWeek > 0
                ? Math.Round(((double)(totalUserBeforeNow - totalUserBeforeLastWeek) / totalUserBeforeNow) * 100, 2)
                : (totalUserBeforeNow > 0 ? 100 : 0);
            double questionRateBeforeNow = totalQuestionLastWeek > 0
                ? Math.Round((double)(totalQuestion - totalQuestionLastWeek) / totalQuestion * 100, 2)
                : (totalQuestion > 0 ? 100 : 0);

            return new AdminWeeklyResponse
            {
                LoginThisWeek = loginThisWeek, // tổng số người đăng nhập trong tuần này
                LoginLastWeek = loginLastWeek, // tổng số người đăng nhập trong tuần trước
                TotalUserBeforeNow = totalUserBeforeNow, // tổng số lượng người dùng
                TotalRevenue = totalRevenue *1000, // tổng doanh thu
                RateRevenue = revenueRate, // tỷ lệ doanh thu so với tuần trước
                LoginRateBeforeNow = loginRateBeforeNow, // tỷ lệ người dùng đăng nhập so với tổng người dùng trước thời điểm hiện tại
                TotalBook = totalBook, // tổng số sách
                TotalQuestion = totalQuestion, // tổng số câu hỏi
                QuestionRate = questionRateBeforeNow, // tỷ lệ câu hỏi so với tuần trước
                UserRateNow = userRateNow // tỉ lệ người dùng mới so với tổng người dùng trước tuần này
            };
        }
        public async Task<InvoiceResponse> GetInvoiceStatistics(InvoiceFilter filter)
            {
            var payments = await _context.Payments.ToListAsync();
            var users = await _context.Users.ToListAsync();

            var invoiceItems = (from p in payments
                                join u in users on p.UserId equals u.Id
                                select new InvoiceItem
                                {
                                    InvoiceId = $"{p.PaymentLinkId}",
                                    FullName = $"{u.FirstName} {u.LastName}",
                                    CreatedAt = p.CreatedAt,
                                    Amount = p.Amount*1000,
                                    PaymentMethod = "PayOS",
                                    Status = p.Status,
                                    AvatarUrl = u.photoURL ?? ""
                                }).ToList();

            var totalCount = invoiceItems.Count;

            var paginatedItems = invoiceItems
                .OrderByDescending(u => u.CreatedAt)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var response = new InvoiceResponse
            {
                TotalBill = payments.Count,
                PendingBill = payments.Count(p => p.Status == PaymentStatus.Pending.ToString()),
                CompletedBill = payments.Count(p => p.Status == PaymentStatus.Completed.ToString()),
                CanceledBill = payments.Count(p =>
                    p.Status == PaymentStatus.Cancelled.ToString() ||
                    p.Status == PaymentStatus.Expired.ToString()),
                Invoices = new Pagination<InvoiceItem>(filter.PageIndex, filter.PageSize, totalCount, paginatedItems)
            };

            return response;
        }
        public async Task<WeeklyRevenueResponse> GetAllWeeklyRevenueAsync()
        {
            var payments = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed.ToString())
                .ToListAsync();

            if (!payments.Any())
            {
                return new WeeklyRevenueResponse(); // Trả về rỗng nếu không có dữ liệu
            }

            var firstPaymentDate = payments.Min(p => p.CreatedAt)!.Value.Date;
            var currentDate = DateTime.UtcNow.Date;

            // Bắt đầu từ ngày Chủ Nhật đầu tiên của tuần có payment đầu tiên
            var startOfWeek = firstPaymentDate.AddDays(-(int)firstPaymentDate.DayOfWeek);

            var weeklyRevenues = new List<WeeklyRevenueItem>();

            while (startOfWeek <= currentDate)
            {
                var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);

                var weeklyRevenue = payments
                    .Where(p => p.CreatedAt >= startOfWeek && p.CreatedAt <= endOfWeek)
                    .Sum(p => p.Amount);

                weeklyRevenues.Add(new WeeklyRevenueItem
                {
                    WeekRange = $"{startOfWeek:yyyy-MM-dd} to {endOfWeek:yyyy-MM-dd}",
                    Revenue = weeklyRevenue*1000
                });

                startOfWeek = startOfWeek.AddDays(7);
            }

            return new WeeklyRevenueResponse
            {
                WeeklyRevenues = weeklyRevenues
            };
        }
    }
}
