using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications.Curriculums
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
                (string.IsNullOrEmpty(param.Search) || curriculum.Name.ToLower().Contains(param.Search.ToLower()));
            if (param.Sort == "Name")
            {
                OrderBy = query => query.OrderBy(c => c.Name);
            }
            else if (param.Sort == "Grade")
            {
                OrderBy = query => query.OrderBy(c => c.Grade);
            }
            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
            Includes.Add(c => c.ContentFlows);
        }
    }
}
