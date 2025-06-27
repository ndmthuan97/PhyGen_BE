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
    public class CurriculumSpecification : ISpecification<Curriculum>
    {
        public Expression<Func<Curriculum, bool>> Criteria { get; private set; }

        public Func<IQueryable<Curriculum>, IOrderedQueryable<Curriculum>>? OrderBy { get; private set; }

        public Func<IQueryable<Curriculum>, IOrderedQueryable<Curriculum>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Curriculum, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Curriculum>, IQueryable<Curriculum>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public CurriculumSpecification(CurriculumSpecParam param)
        {
            Criteria = curriculum =>
                (string.IsNullOrEmpty(param.Search) || curriculum.Name.ToLower().Contains(param.Search.ToLower()))
                && !curriculum.DeletedAt.HasValue;

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
