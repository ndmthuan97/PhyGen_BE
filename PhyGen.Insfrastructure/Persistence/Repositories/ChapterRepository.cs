using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class ChapterRepository : RepositoryBase<Chapter, Guid>, IChapterRepository
    {
        public ChapterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Chapter?> GetChapterByTitleAsync(string title)
        {
            return await _context.Chapters
                .FirstOrDefaultAsync(c =>
                    EF.Functions.Collate(c.Title, "Latin1_General_100_CI_AI_SC") == title);
        }
    }
}
