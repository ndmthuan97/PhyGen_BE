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
    public class ContentFlowRepository :RepositoryBase<ContentFlow, Guid>, IContentFlowRepository
    {
        public ContentFlowRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ContentFlow>> GetContentFlowsByCurriculumIdAsync(Guid curriculumId)
        {
            return await _context.ContentFlows
                .Where(cf => cf.CurriculumId == curriculumId)
                .ToListAsync();
        }
    }
}
