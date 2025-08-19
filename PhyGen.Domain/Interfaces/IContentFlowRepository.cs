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
        Task<List<ContentFlow>> GetContentFlowsByCurriculumIdAndSubjectIdAsync(Guid curriculumId, Guid subjectId);
        Task<int> GetMaxOrderNoByCurriculumIdAndSubjectIdAsync(Guid curriculumId, Guid subjectId, int grade);
    }
}
