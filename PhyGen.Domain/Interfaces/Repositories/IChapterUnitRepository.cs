using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface IChapterUnitRepository : IAsyncRepository<ChapterUnit, Guid>
    {
        Task<ChapterUnit?> GetChapterUnitByTitleAsync(string title);
        Task<List<ChapterUnit>> GetChapterUnitsByChapterIdAsync(Guid chapterId);
    }
}
