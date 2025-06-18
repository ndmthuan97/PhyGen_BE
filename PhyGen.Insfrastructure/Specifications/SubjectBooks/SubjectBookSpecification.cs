using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.SubjectBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications.SubjectBooks
{
    public class SubjectBookSpecification : ISpecification<SubjectBook>
    {
        public Expression<Func<SubjectBook, bool>> Criteria { get; private set; }
        public Func<IQueryable<SubjectBook>, IOrderedQueryable<SubjectBook>>? OrderBy { get; private set; }
        public Func<IQueryable<SubjectBook>, IOrderedQueryable<SubjectBook>>? OrderByDescending { get; private set; }
        public List<Expression<Func<SubjectBook, object>>> Includes { get; private set; } = [];
        public Func<IQueryable<SubjectBook>, IQueryable<SubjectBook>>? Selector { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public SubjectBookSpecification(SubjectBookSpecParam param)
        {
            Criteria = subjectBook =>
                (param.SubjectId == Guid.Empty || subjectBook.SubjectId == param.SubjectId) &&
                (string.IsNullOrEmpty(param.Search) || subjectBook.Name.ToLower().Contains(param.Search.ToLower()));

            if(!string.IsNullOrEmpty(param.Search))
            {
                switch (param.Search.ToLower())
                {
                    case "name":
                        OrderBy = query => query.OrderBy(sb => sb.Name);
                        break;
                    case "namedesc":
                        OrderByDescending = query => query.OrderByDescending(sb => sb.Name);
                        break;
                    case "grade":
                        OrderBy = query => query.OrderBy(sb => sb.Grade);
                        break;
                    case "gradedesc":
                        OrderByDescending = query => query.OrderByDescending(sb => sb.Grade);
                        break;
                    default:
                        OrderBy = query => query.OrderBy(sb => sb.Name);
                        break;
                }
            } else
            {
                OrderBy = query => query.OrderBy(sb => sb.Name);
            }

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
