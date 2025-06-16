using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IMatrixRepository : IAsyncRepository<Matrix, Guid>
    {
        Task<Matrix?> GetBySubjectCurriculumIdAndExamCategoryIdAsync(Guid subjectCurriculumId, Guid examCategoryId);
    }
}
