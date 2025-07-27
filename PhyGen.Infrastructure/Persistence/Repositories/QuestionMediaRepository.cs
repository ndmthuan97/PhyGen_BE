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
    public class QuestionMediaRepository : RepositoryBase<QuestionMedia, Guid>, IQuestionMediaRepository
    {
        public QuestionMediaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<QuestionMedia>> GetQuestionMediaByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuestionMedias
                .Where(x => x.QuestionId == questionId)
                .ToListAsync();
        }
    }
}
