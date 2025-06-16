using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IQuestionRepository : IAsyncRepository<Question, Guid>
    {
        Task<List<Question>> GetByChapterUnitIdAsync(Guid chapterUnitId);
        Task<List<Question>> GetByTypeAsync(string type);
        Task<List<Question>> GetByLevelAsync(string level);
    }
}
