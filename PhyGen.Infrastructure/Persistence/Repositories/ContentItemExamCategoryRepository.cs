using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ContentItemExamCategoryRepository : RepositoryBase<ContentItemExamCategory, Guid>, IContentItemExamCategoryRepository
    {
        public ContentItemExamCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public Task<ContentItemExamCategory?> GetContentItemExamCategoryByContentItemIdAndExamCategoryIDAsync(Guid contentItemId, Guid examCategoryId)
        {
            return _context.ContentItemExamCategories
                .FirstOrDefaultAsync(ciec => ciec.ContentItemId == contentItemId && ciec.ExamCategoryId == examCategoryId);
        }
    }
}
