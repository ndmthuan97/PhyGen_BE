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
    public class AnswerRepository : RepositoryBase<Answer, Guid>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Answer?> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == questionId);
        }
    }
}
