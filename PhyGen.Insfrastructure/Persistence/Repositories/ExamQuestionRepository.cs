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
    public class ExamQuestionRepository : RepositoryBase<ExamQuestion, Guid>, IExamQuestionRepository
    {
        public ExamQuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ExamQuestion?> GetByExamIdAndQuestionIdAsync(Guid examId, Guid questionId)
        {
            return await _context.Set<ExamQuestion>()
                .FirstOrDefaultAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);
        }

        public async Task<List<ExamQuestion>> GetByExamIdAsync(Guid examId)
        {
            return await _context.Set<ExamQuestion>()
                .Where(eq => eq.ExamId == examId)
                .ToListAsync();
        }

        public async Task<List<ExamQuestion>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.Set<ExamQuestion>()
                .Where(eq => eq.QuestionId == questionId)
                .ToListAsync();
        }
    }
}
