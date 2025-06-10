using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IExamCategoryChapterRepository : IAsyncRepository<ExamCategoryChapter, Guid>
    {
        Task<ExamCategoryChapter?> GetByExamCategoryIdOrChapterIdAsync(Guid examCategoryId, Guid chapterId);
    }
}
