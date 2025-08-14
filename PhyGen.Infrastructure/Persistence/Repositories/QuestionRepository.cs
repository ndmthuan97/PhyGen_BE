using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using PhyGen.Infrastructure.Specifications.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class QuestionRepository : RepositoryBase<Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context) { }

        public async Task<Pagination<Question>?> GetQuestionsAsync(QuestionSpecParam questionSpecParam)
        {
            var spec = new QuestionSpecification(questionSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<Pagination<Question>?> GetQuestionsByTopicAsync(QuestionByTopicSpecParam param)
        {
            var spec = new QuestionByTopicSpecification(param);
            return await GetWithSpecAsync(spec);
        }

        public async Task<Pagination<Question>?> GetQuestionsByLevelAndTopicAsync(QuestionSpecParam questionSpecParam)
        {
            var spec = new QuestionSpecification(questionSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<Pagination<Question>?> GetQuestionsByGradeAsync(QuestionByGradeSpecParam questionGradeSpecParam)
        {
            var spec = new QuestionByGradeSpecification(questionGradeSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<string> GenerateQuestionCodeAsync()
        {
            var last = await _context.Questions
                .Where(q => !string.IsNullOrEmpty(q.QuestionCode) && q.QuestionCode.StartsWith("Q"))
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => q.QuestionCode)
                .FirstOrDefaultAsync();

            int next = 1;
            if (!string.IsNullOrEmpty(last))
            {
                var digits = last.Substring(1);
                if (int.TryParse(digits, out var n))
                    next = n + 1;
            }

            return $"Q{next:D4}";
        }
    }
}
