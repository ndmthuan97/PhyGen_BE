using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ISectionRepository : IAsyncRepository<Section, Guid>
    {
        Task<Section?> GetSectionsByExamIdAsync(Guid examId);
    }
}
