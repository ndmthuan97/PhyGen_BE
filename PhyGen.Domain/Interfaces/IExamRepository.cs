using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IExamRepository : IAsyncRepository<Exam, Guid>
    {
        Task<Exam?> GetByCreatorAsync(string createdBy);
        Task<Exam?> GetBySubjectCurriculumIdOrMatrixIdAsync(Guid subjectCurriculumId, Guid matrixId);
    }
}
