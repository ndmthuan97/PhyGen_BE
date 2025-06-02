using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class ExamRepository : RepositoryBase<Exam, Guid>, IExamRepository
    {
        public ExamRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Exam?> GetExamByTitleAsync(string examTitle)
        {
            return await _context.Exams
                .FirstOrDefaultAsync(c =>
                    EF.Functions.Collate(c.Title, "Latin1_General_100_CI_AI_SC") == examTitle);
        }
    }
}
