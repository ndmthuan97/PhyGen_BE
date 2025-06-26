using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification, int>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Notification>> GetByUserIdAsync(Guid? id)
        {
            if (id == null)
                return new List<Notification>();

            return await _context.Notifications
                .Where(n => n.UserId == id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<(List<Notification>, int)> GetByUserIdAsync(Guid userId, int skip, int take)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt); // hoặc UpdatedAt tùy bạn

            var total = await query.CountAsync();
            var data = await query.Skip(skip).Take(take).ToListAsync();

            return (data, total);
        }

    }
}
