using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ITopicRepository : IAsyncRepository<Topic, Guid>
    {
        Task<Topic?> GetTopicByChapterIdAndNameAsync(Guid chapterId, string name);

        Task<List<Topic>> GetTopicsByChapterIdAsync(Guid chapterId);
    }
}
