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
    public class QuestionMediaRepository : RepositoryBase<QuestionMedia, Guid>, IQuestionMediaRepository
    {
        public QuestionMediaRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<QuestionMedia>> GetByQuestionIdAsync(Guid questionId)
        {
            return _context.Set<QuestionMedia>()
                .Where(qm => qm.QuestionId == questionId)
                .ToListAsync();
        }
    }
}
