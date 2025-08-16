using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ISubjectBookRepository : IAsyncRepository<SubjectBook, Guid>
    {
        Task<Pagination<SubjectBook>?> GetSubjectBooksBySubjectWithSpecAsync(SubjectBookSpecParam subjectBookSpecParam);
        Task<List<SubjectBook>> GetSubjectBooksByGradeAsync(int grade);
        Task<object?> GetNamesByTopicIdAsync(Guid topicId);
    }
}
