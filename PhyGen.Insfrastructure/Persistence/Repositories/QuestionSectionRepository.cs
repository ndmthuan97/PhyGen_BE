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
    public class QuestionSectionRepository : RepositoryBase<QuestionSection, Guid>, IQuestionSectionRepository
    {
        public QuestionSectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<QuestionSection?> GetQuestionSectionByQuestionIdAndSectionIdAsync(Guid questionId, Guid sectionId)
        {
            return await _context.QuestionSections
                .FirstOrDefaultAsync(qs => qs.QuestionId == questionId && qs.SectionId == sectionId && qs.DeletedAt == null);
        }
    }
}
