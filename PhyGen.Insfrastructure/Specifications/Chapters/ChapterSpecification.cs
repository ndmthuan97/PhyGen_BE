using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications.Chapters
{
    public class ChapterSpecification : ISpecification<Chapter>
    {
        public Expression<Func<Chapter, bool>> Criteria { get; private set; }
        public Func<IQueryable<Chapter>, IOrderedQueryable<Chapter>>? OrderBy { get; private set; }
        public Func<IQueryable<Chapter>, IOrderedQueryable<Chapter>>? OrderByDescending { get; private set; }
        public List<Expression<Func<Chapter, object>>> Includes { get; private set; } = [];
        public Func<IQueryable<Chapter>, IQueryable<Chapter>>? Selector { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
    }
}
