using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ContentItemRepository : RepositoryBase<ContentItem, Guid>, IContentItemRepository
    {
        public ContentItemRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<ContentItem>> GetContentItemsByContentFlowIdAsync(Guid contentFlowId)
        {
            return _context.ContentItems
                .Where(ci => ci.ContentFlowId == contentFlowId)
                .ToListAsync();
        }

        public async Task<int> GetMaxOrderNoByContentFlowIdAsync(Guid contentFlowId)
        {
            return await _context.ContentItems
                .Where(ci => ci.ContentFlowId == contentFlowId && ci.DeletedAt == null)
                .Select(ci => (int?)ci.OrderNo)
                .MaxAsync() ?? 0;
        }
    }
}
