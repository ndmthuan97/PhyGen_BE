using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications
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

        public ChapterSpecification(ChapterSpecParam param)
        {
            Criteria = chapter =>
                chapter.SubjectBookId == param.SubjectBookId &&
                (string.IsNullOrEmpty(param.Search) || chapter.Name.ToLower().Contains(param.Search.ToLower())) &&
                !chapter.DeletedAt.HasValue;
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                        OrderBy = query => query.OrderBy(c => c.Name);
                        break;
                    case "namedesc":
                        OrderByDescending = query => query.OrderByDescending(c => c.Name);
                        break;
                    default:
                        OrderBy = query => query.OrderBy(c => c.Name);
                        break;
                }
            }
            else
            {
                OrderBy = query => query.OrderBy(c => c.Name);
            }
            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
