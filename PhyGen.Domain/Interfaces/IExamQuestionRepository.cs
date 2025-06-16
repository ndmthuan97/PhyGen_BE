using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IExamQuestionRepository : IAsyncRepository<ExamQuestion, Guid>
    {
        Task<ExamQuestion?> GetByExamIdAndQuestionIdAsync(Guid examId, Guid questionId);
        Task<List<ExamQuestion>> GetByExamIdAsync(Guid examId);
        Task<List<ExamQuestion>> GetByQuestionIdAsync(Guid questionId);
    }
}
