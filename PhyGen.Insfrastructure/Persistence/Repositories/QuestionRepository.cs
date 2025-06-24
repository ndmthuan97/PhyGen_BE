using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using PhyGen.Infrastructure.Specifications.Questions;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
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
    }
}
