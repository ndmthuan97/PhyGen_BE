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
    public class ExamCategoryChapterRepository : RepositoryBase<ExamCategoryChapter, Guid>, IExamCategoryChapterRepository
    {
        public ExamCategoryChapterRepository(AppDbContext context) : base(context)
        {
        }

        public Task<ExamCategoryChapter?> GetByExamCategoryIdAndChapterIdAsync(Guid examCategoryId, Guid chapterId)
        {
            return _context.ExamCategoryChapters
                .FirstOrDefaultAsync(ecc => ecc.ExamCategoryId == examCategoryId && ecc.ChapterId == chapterId);
        }
    }
}
