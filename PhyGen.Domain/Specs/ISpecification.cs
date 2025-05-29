using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    // Define query criteria for Entity according to Specification pattern. 
    // Allows to configure filtering, sorting, including relations, selecting fields, and pagination.
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; }
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByDescending { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? Selector { get; }
        int Skip { get; }
        int Take { get; }
    }
}
