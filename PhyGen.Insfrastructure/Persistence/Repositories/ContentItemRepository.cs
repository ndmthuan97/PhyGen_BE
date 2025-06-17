using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
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

        public Task<ContentItem?> GetContentItemByContentFlowIdAndNameAsync(Guid contentFlowId, string name)
        {
            return _context.ContentItems
                .FirstOrDefaultAsync(ci => ci.ContentFlowId == contentFlowId && ci.Name.ToLower() == name.ToLower());
        }

        public Task<List<ContentItem>> GetContentItemsByContentFlowIdAsync(Guid contentFlowId)
        {
            return _context.ContentItems
                .Where(ci => ci.ContentFlowId == contentFlowId)
                .ToListAsync();
        }
    }
}
