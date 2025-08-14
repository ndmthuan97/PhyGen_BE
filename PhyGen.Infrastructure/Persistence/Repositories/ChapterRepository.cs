using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Specifications;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ChapterRepository : RepositoryBase<Chapter, Guid>, IChapterRepository
    {
        public ChapterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<Chapter>?> GetChaptersBySubjectBookAsync(ChapterSpecParam chapterSpecParam)
        {
            var spec = new ChapterSpecification(chapterSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<string> GetChapterCodeAsync()
        {
            var lastChapter = await _context.Chapters
                .OrderByDescending(c => c.ChapterCode)
                .FirstOrDefaultAsync();

            if (lastChapter == null || string.IsNullOrEmpty(lastChapter.ChapterCode))
            {
                return "C0001";
            }

            if (!int.TryParse(lastChapter.ChapterCode.Substring(1), out int lastNumber))
            {
                lastNumber = 0;
            }

            int newNumber = lastNumber + 1;
            return $"C{newNumber:D4}";
        }
    }
}
