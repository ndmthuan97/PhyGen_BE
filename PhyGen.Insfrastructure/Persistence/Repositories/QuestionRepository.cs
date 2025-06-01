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
    public class QuestionRepository : RepositoryBase<Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Question?> GetQuestionByContentAsync(string questionContent)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(c =>
                    EF.Functions.Collate(c.Content, "Latin1_General_100_CI_AI_SC") == questionContent);
        }
    }
}
