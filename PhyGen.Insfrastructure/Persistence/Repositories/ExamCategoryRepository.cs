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
    public class ExamCategoryRepository : RepositoryBase<ExamCategory, Guid>, IExamCategoryRepository
    {
        public ExamCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
