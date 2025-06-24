using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.BackgroundServices
{
    public class ExpirePaymentBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // Cấu hình: mỗi 5 phút

        public ExpirePaymentBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var now = DateTime.UtcNow;

                    var expiredPayments = await dbContext.Payments
                        .Where(p => p.Status == PaymentStatus.Pending.ToString() && p.ValidUntil < now)
                        .ToListAsync(stoppingToken);

                    foreach (var payment in expiredPayments)
                    {
                        payment.Status = PaymentStatus.Expired.ToString();
                    }

                    if (expiredPayments.Any())
                    {
                        await dbContext.SaveChangesAsync(stoppingToken);
                        Console.WriteLine($"[ExpirePayments] Đã cập nhật {expiredPayments.Count} payment hết hạn.");
                    }
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
