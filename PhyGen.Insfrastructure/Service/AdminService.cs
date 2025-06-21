using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Admin.Interfaces;
using PhyGen.Application.Admin.Response;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var revenueThisWeek = await _context.Payments
                .Where(p => p.CreatedAt >= startOfThisWeek && p.Status == "PAID")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var revenueLastWeek = await _context.Payments
                .Where(p => p.CreatedAt >= startOfLastWeek && p.CreatedAt <= endOfLastWeek && p.Status == "PAID")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var toalrevenue = await _context.Payments
                .Where(p => p.CreatedAt >= now)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Tổng người dùng trước tuần này
            var totalUserBeforeThisWeek = await _context.Users
                .CountAsync(u => u.CreatedAt < startOfThisWeek);

            // Tổng login trước tuần trước
            var totalLoginBeforeLastWeek = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin < startOfLastWeek);

            // Tổng trước thời điểm hiện tại
            var totalUserBeforeNow = await _context.Users
                .CountAsync(u => u.CreatedAt < now);

            var totalLoginBeforeNow = await _context.Users
                .CountAsync(u => u.LastLogin.HasValue && u.LastLogin < now);

            double revenueRate = (double)(revenueThisWeek > 0
                ? Math.Round((revenueThisWeek - revenueLastWeek) / revenueThisWeek * 100, 2)
                : 0);
            // Tính tỷ lệ login
            double loginRateBeforeNow = totalUserBeforeNow > 0
                ? Math.Round((double)totalLoginBeforeNow / totalUserBeforeNow * 100, 2)
                : 0;

            double loginRateBeforeLastWeek = totalUserBeforeThisWeek > 0
                ? Math.Round((double)totalLoginBeforeLastWeek / totalUserBeforeThisWeek * 100, 2)
                : 0;

            return new AdminWeeklyResponse
            {
                LoginThisWeek = loginThisWeek,
                LoginLastWeek = loginLastWeek,
                TotalUserBeforeNow = totalUserBeforeNow,
                TotalRevenue = toalrevenue,
                RateRevenue = revenueRate,
                LoginRateBeforeNow = loginRateBeforeNow,
                LoginRateBeforeLastWeek = loginRateBeforeLastWeek
            };
        }
    }
}
