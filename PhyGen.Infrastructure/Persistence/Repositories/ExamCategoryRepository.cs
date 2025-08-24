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
    public class ExamCategoryRepository : RepositoryBase<ExamCategory, Guid>, IExamCategoryRepository
    {
        public ExamCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> GetOrderNoMaxAsync()
        {
            var maxOrderNo = await _context.ExamCategories.MaxAsync(ec => (int?)ec.OrderNo) ?? 0;
            return maxOrderNo;
        }
    }
}
