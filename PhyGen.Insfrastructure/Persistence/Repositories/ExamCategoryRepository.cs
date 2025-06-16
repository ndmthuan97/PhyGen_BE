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
    public class ExamCategoryRepository : RepositoryBase<ExamCategory, int>, IExamCategoryRepository
    {
        public ExamCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ExamCategory?> GetByNameAsync(string name)
        {
            return await _context.ExamCategories
                .FirstOrDefaultAsync(ec => ec.Name.ToLower() == name.ToLower());
        }
    }
}
