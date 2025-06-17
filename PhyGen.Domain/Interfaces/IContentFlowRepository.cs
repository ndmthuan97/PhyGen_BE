using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IContentFlowRepository : IAsyncRepository<ContentFlow, Guid>
    {
        Task<ContentFlow?> GetContentFlowByCurriculumIdAndNameAsync(Guid curriculumId, string name);

        Task<List<ContentFlow>> GetContentFlowsByCurriculumIdAsync(Guid curriculumId);
    }
}
