using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class QuestionRepository : RepositoryBase<Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Question>> GetByChapterUnitIdAsync(Guid chapterUnitId)
        {
            return await _context.Set<Question>()
                .Where(q => q.ChapterUnitId == chapterUnitId)
                .ToListAsync();
        }

        public async Task<List<Question>> GetByTypeAsync(string type)
        {
            return await _context.Set<Question>()
                .Where(q => q.Type == type)
                .ToListAsync();
        }

        public async Task<List<Question>> GetByLevelAsync(string level)
        {
            return await _context.Set<Question>()
                .Where(q => q.Level == level)
                .ToListAsync();
        }
    }
}
