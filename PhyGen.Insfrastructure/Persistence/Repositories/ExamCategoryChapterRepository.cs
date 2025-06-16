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
    public class ExamCategoryChapterRepository : RepositoryBase<ExamCategoryChapter, Guid>, IExamCategoryChapterRepository
    {
        public ExamCategoryChapterRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<ExamCategoryChapter?> GetByExamCategoryIdOrChapterIdAsync(Guid examCategoryId, Guid chapterId)
        {
            return await _context.Set<ExamCategoryChapter>()
                .FirstOrDefaultAsync(ecc => ecc.ExamCategoryId == examCategoryId || ecc.ChapterId == chapterId);
        }
    }
}
