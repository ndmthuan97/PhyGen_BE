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
    public class ContentItemExamCategoryRepository : RepositoryBase<ContentItemExamCategory, int>, IContentItemExamCategoryRepository
    {
        public ContentItemExamCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ContentItemExamCategory?> GetByContentItemIdAndExamCategoryIdAsync(Guid contentItemId, int examCategoryId)
        {
            return await _context.ContentItemExamCategories
                .FirstOrDefaultAsync(ciec => ciec.ContentItemId == contentItemId && ciec.ExamCategoryId == examCategoryId);
        }
    }
}
