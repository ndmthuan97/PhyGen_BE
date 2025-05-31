using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface IAnswerRepository : IAsyncRepository<Answer, Guid>
    {
        //Task<List<Answer?>> GetAllAnswerAsync();
        Task<Answer?> GetByQuestionIdAsync(Guid questionId);
        //Task AddAsync(Answer answer);
        //Task UpdateAsync(Answer answer);
        //Task DeleteAsync(Guid id);
    }
}
