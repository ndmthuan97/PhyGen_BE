using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.SubjectBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ISubjectBookRepository : IAsyncRepository<SubjectBook, Guid>
    {
        Task<SubjectBook?> GetSubjectBookByNameAsync(string name);
        Task<List<SubjectBook>> GetSubjectBooksBySubjectIdAsync(Guid subjectId);
        Task<Pagination<SubjectBook>?> GetSubjectBooksAsync(SubjectBookSpecParam subjectBookSpecParam);
        Task<Pagination<SubjectBook>?> GetSubjectBooksBySubjectIdWithSpecAsync(SubjectBookBySubjectIdSpecParam subjectBookBySubjectIdSpecParam);
    }
}
