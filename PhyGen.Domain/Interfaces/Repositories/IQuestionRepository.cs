using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface IQuestionRepository : IAsyncRepository<Question, Guid>
    {
        Task<List<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(Guid id);
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(Guid id, Guid deletedByUserId);

        //Task<List<Question>> GetByChapterIdAsync(Guid chapterId);
        //Task<List<Question>> GetByCreatedUserAsync(Guid userId);
        //Task<List<Question>> GetPagedAsync(int pageIndex, int pageSize);
    }
}
