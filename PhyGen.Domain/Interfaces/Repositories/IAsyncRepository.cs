using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    // Interface defines asynchronous data manipulation methods for Entities.
    // Apply generics to Entities of type TEntity and primary keys of type TKey.
    public interface IAsyncRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        // Get data according to Specification pattern with filtering, sorting, including relations, selecting fields and pagination conditions.
        Task<Pagination<TEntity>> GetWithSpecAsync<TSpec>(TSpec spec) where TSpec : ISpecification<TEntity>;
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
