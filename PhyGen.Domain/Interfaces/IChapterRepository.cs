using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IChapterRepository : IAsyncRepository<Chapter, Guid>
    {
        Task<Pagination<Chapter>?> GetChaptersBySubjectBookAsync(ChapterSpecParam chapterSpecParam);
        Task<string> GetChapterCodeAsync();
    }
}
