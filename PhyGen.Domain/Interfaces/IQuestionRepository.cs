using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IQuestionRepository : IAsyncRepository<Question, Guid>
    {
        Task<Pagination<Question>?> GetQuestionsAsync(QuestionSpecParam questionSpecParam);

        Task<Pagination<Question>?> GetQuestionsByTopicAsync(QuestionByTopicSpecParam param);

        Task<Pagination<Question>?> GetQuestionsByLevelAndTopicAsync(QuestionSpecParam questionSpecParam);
    }
}
