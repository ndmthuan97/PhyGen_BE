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

        public async Task<Chapter?> GetChapterByNameAsync(string name)
        {
            return await _context.Chapters
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Chapter>> GetChaptersByBookIdAsync(Guid bookId)
        {
            return await _context.BookDetails
                .Where(bd => bd.BookId == bookId)
                .Select(bd => bd.Chapter)
                .ToListAsync();
        }

        public async Task<List<Chapter>> GetChaptersByCurriculumIdAsync(Guid curriculumId)
        {
            return await _context.Chapters
                .Where(c => c.CurriculumId == curriculumId)
                .ToListAsync();
        }
    }
}
