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
    public class MatrixSpecification : ISpecification<Matrix>
    {
        public Expression<Func<Matrix, bool>> Criteria { get; private set; }

        public Func<IQueryable<Matrix>, IOrderedQueryable<Matrix>>? OrderBy { get; private set; }

        public Func<IQueryable<Matrix>, IOrderedQueryable<Matrix>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Matrix, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Matrix>, IQueryable<Matrix>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public MatrixSpecification(MatrixSpecParam param)
        {
            var examCategory = param.ExamCategory?
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim().ToLower())
                .ToList() ?? new List<string>();

            Criteria = matrix =>
                (string.IsNullOrEmpty(param.Search) || matrix.Name.ToLower().Contains(param.Search.ToLower())) &&                
                (!param.SubjectId.HasValue || matrix.SubjectId == param.SubjectId) &&
                (!param.ExamCategoryId.HasValue || matrix.ExamCategoryId == param.ExamCategoryId) &&
                (!examCategory.Any() || examCategory.Any(n => matrix.ExamCategory.Name.Trim().ToLower().Contains(n))) &&
                (param.Grade == null || !param.Grade.Any() || param.Grade.Contains(matrix.Grade)) &&
                (param.Year == null || !param.Year.Any() || param.Year.Contains(matrix.Year)) &&
                (param.Status == null || matrix.Status == param.Status.Value) &&
                (string.IsNullOrEmpty(param.MatrixCode) || matrix.MatrixCode.Contains(param.MatrixCode)) &&
                (!matrix.DeletedAt.HasValue);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {                    
                    case "year":
                        OrderBy = query => query.OrderBy(m => m.Year);
                        break;
                    case "yeardesc":
                        OrderByDescending = query => query.OrderByDescending(m => m.Year);
                        break;
                    case "examcategory":
                        OrderBy = query => query.OrderBy(m => m.ExamCategory.Name);
                        break;
                    case "examcategorydesc":
                        OrderByDescending = query => query.OrderByDescending(m => m.ExamCategory.Name);
                        break;
                    case "createdat":
                        OrderBy = query => query.OrderBy(m => m.CreatedAt);
                        break;
                    case "createdatdesc":
                        OrderByDescending = query => query.OrderByDescending(m => m.CreatedAt);
                        break;
                    default:
                        OrderByDescending = query => query.OrderByDescending(m => m.CreatedAt);
                        break;
                }
            }

            Includes.Add(m => m.Subject);
            Includes.Add(m => m.ExamCategory);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
