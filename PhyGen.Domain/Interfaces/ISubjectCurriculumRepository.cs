using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ISubjectCurriculumRepository : IAsyncRepository<SubjectCurriculum, Guid>
    {
        Task<SubjectCurriculum?> GetBySubjectIdAndCurriculumIdAsync(Guid subjectId, Guid curriculumId);
    }
}
