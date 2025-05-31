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
    public class ChapterUnitRepository : RepositoryBase<ChapterUnit, Guid>, IChapterUnitRepository
    {
        public ChapterUnitRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ChapterUnit?> GetChapterUnitByTitleAsync(string title)
        {
            return await _context.ChapterUnits
                .FirstOrDefaultAsync(cu => cu.Title.ToLower() == title.ToLower());
        }

        public async Task<List<ChapterUnit>> GetChapterUnitsByChapterIdAsync(Guid chapterId)
        {
            return await _context.ChapterUnits
                .Where(cu => cu.ChapterId == chapterId)
                .ToListAsync();
        }
    }
}
