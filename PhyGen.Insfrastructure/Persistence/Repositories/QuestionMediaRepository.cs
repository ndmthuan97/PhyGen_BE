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
    public class QuestionMediaRepository : RepositoryBase<QuestionMedia, Guid>, IQuestionMediaRepository
    {
        public QuestionMediaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<QuestionMedia?> GetQuestionMediaByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuestionMedias
                .FirstOrDefaultAsync(qm => qm.QuestionId == questionId);
        }
    }
}
