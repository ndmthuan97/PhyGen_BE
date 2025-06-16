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
    public class ContentFlowRepository : RepositoryBase<ContentFlow, int>, IContentFlowRepository
    {
        public ContentFlowRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ContentFlow?> GetContentFlowByNameAsync(string name)
        {
            return await _context.ContentFlows
                .FirstOrDefaultAsync(cf => cf.Name.ToLower() == name.ToLower());
        }
    }
}
