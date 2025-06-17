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
    public class ChapterRepository : RepositoryBase<Chapter, Guid>, IChapterRepository
    {
        public ChapterRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Chapter?> GetChapterBySubjectBookIdAndNameAsync(Guid subjectBookId, string name)
        {
            return _context.Chapters
                .FirstOrDefaultAsync(c => c.SubjectBookId == subjectBookId && c.Name.ToLower() == name.ToLower());
        }

        public Task<List<Chapter>> GetChaptersBySubjectBookIdAsync(Guid subjectBookId)
        {
            return _context.Chapters
                .Where(c => c.SubjectBookId == subjectBookId)
                .ToListAsync();
        }
    }
}
