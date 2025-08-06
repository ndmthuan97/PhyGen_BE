using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ITopicRepository : IAsyncRepository<Topic, Guid>
    {
        Task<Pagination<Topic>?> GetTopicsByChapterAsync(TopicSpecParam topicSpecParam);
        Task<List<Topic>> GetValidTopicsAsync();

        Task<int?> GetGradeByTopicIdAsync(Guid id);
        Task<List<Topic>> GetTopicsByGradeAsync(TopicByGradeSpecParam topicByGradeSpecParam);
    }
}
