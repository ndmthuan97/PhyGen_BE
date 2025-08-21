using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IQuestionSectionRepository : IAsyncRepository<QuestionSection, Guid>
    {
        Task<QuestionSection?> GetQuestionSectionByQuestionIdAndSectionIdAsync(Guid questionId,Guid sectionId);
    }
}
