using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class ContentItemRepository : RepositoryBase<ContentItem, Guid>, IContentItemRepository
    {
        public ContentItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ContentItem?> GetContentItemByTitleAsync(string title)
        {
            return await _context.ContentItems
                .FirstOrDefaultAsync(ci => ci.Title.ToLower() == title.ToLower());
        }
    }
}
