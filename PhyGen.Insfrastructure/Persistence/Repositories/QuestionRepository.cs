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

        public async Task<List<Question>> GetAllAsync()
        {
            return await _context.Questions
                .Where(q => q.DeletedAt == null) // Avoid soft-deleted questions
                .Include(q => q.Chapter)
                .Include(q => q.Creator)
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid id)
        {
            return await _context.Questions
                .Include(q => q.Chapter)
                .Include(q => q.Creator)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        // Soft delete
        public async Task DeleteAsync(Guid id, Guid deletedByUserId)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                question.DeletedAt = DateTime.UtcNow;
                question.DeletedBy = deletedByUserId.ToString(); 
                _context.Questions.Update(question);
                await _context.SaveChangesAsync();
            }
        }

    }
}
