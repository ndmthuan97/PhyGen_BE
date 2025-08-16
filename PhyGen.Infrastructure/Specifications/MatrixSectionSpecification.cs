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
    public class MatrixSectionSpecification : ISpecification<MatrixSection>
    {
        public Expression<Func<MatrixSection, bool>> Criteria { get; private set; }

        public Func<IQueryable<MatrixSection>, IOrderedQueryable<MatrixSection>>? OrderBy { get; private set; }

        public Func<IQueryable<MatrixSection>, IOrderedQueryable<MatrixSection>>? OrderByDescending { get; private set; }

        public List<Expression<Func<MatrixSection, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<MatrixSection>, IQueryable<MatrixSection>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public MatrixSectionSpecification(MatrixSectionSpecParam param)
        {
            Criteria = section =>
                (string.IsNullOrEmpty(param.Search) || section.Title.ToLower().Contains(param.Search.ToLower())) &&
                (!param.MatrixId.HasValue || section.MatrixId == param.MatrixId);
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "title":
                        OrderBy = query => query.OrderBy(s => s.Title);
                        break;
                    case "titledesc":
                        OrderByDescending = query => query.OrderByDescending(s => s.Title);
                        break;
                }
            } else
            {
                OrderBy = query => query.OrderBy(s => s.Title);
            }

            Includes.Add(s => s.Matrix);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
