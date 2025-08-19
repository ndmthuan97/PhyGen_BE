using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IContentItemRepository : IAsyncRepository<ContentItem, Guid>
    {
        Task<List<ContentItem>> GetContentItemsByContentFlowIdAsync(Guid contentFlowId);
        Task<int> GetMaxOrderNoByContentFlowIdAsync(Guid contentFlowId);
    }
}
