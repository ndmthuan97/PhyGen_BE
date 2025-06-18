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
    public class SubjectBookBySubjectIdSpecification : ISpecification<SubjectBook>
    {
        public Expression<Func<SubjectBook, bool>> Criteria { get; private set; }
        public Func<IQueryable<SubjectBook>, IOrderedQueryable<SubjectBook>>? OrderBy { get; private set; }
        public Func<IQueryable<SubjectBook>, IOrderedQueryable<SubjectBook>>? OrderByDescending { get; private set; }
        public List<Expression<Func<SubjectBook, object>>> Includes { get; private set; } = [];
        public Func<IQueryable<SubjectBook>, IQueryable<SubjectBook>>? Selector { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public SubjectBookBySubjectIdSpecification(SubjectBookBySubjectIdSpecParam param)
        {
            Criteria = x => x.SubjectId == param.SubjectId;

            OrderBy = query => query.OrderBy(sb => sb.Name);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
