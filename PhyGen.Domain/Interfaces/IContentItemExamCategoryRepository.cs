using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IContentItemExamCategoryRepository : IAsyncRepository<ContentItemExamCategory, int>
    {
        Task<ContentItemExamCategory?> GetByContentItemIdAndExamCategoryIdAsync(Guid contentItemId, int examCategoryId);
    }
}
