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
    public class SectionRepository : RepositoryBase<Section, Guid>, ISectionRepository
    {
        public SectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Section?> GetSectionsByExamIdAsync(Guid examId)
        {
            return await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == examId && s.DeletedAt == null);
        }
    }
}
