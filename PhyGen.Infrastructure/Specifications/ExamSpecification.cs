using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace PhyGen.Infrastructure.Specifications
{
    public class ExamSpecification : ISpecification<Exam>
    {
        public Expression<Func<Exam, bool>> Criteria { get; private set; }

        public Func<IQueryable<Exam>, IOrderedQueryable<Exam>>? OrderBy { get; private set; }

        public Func<IQueryable<Exam>, IOrderedQueryable<Exam>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Exam, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Exam>, IQueryable<Exam>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public ExamSpecification(ExamSpecParam param)
        {
            var examCategory = param.ExamCategory?
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim().ToLower())
                .ToList() ?? new List<string>();
            Criteria = exam =>
                (!param.ExamCategoryId.HasValue || exam.ExamCategoryId == param.ExamCategoryId) &&
                (!examCategory.Any() || examCategory.Any(n => exam.ExamCategory.Name.Trim().ToLower().Contains(n))) &&
                (param.Grade == null || !param.Grade.Any() || param.Grade.Contains(exam.Grade)) &&
                (param.Year == null || !param.Year.Any() || param.Year.Contains(exam.Year)) &&
                (string.IsNullOrEmpty(param.Search) || exam.Title.ToLower().Contains(param.Search.ToLower())) &&
                !exam.DeletedAt.HasValue;

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "title":
                        OrderBy = query => query.OrderBy(e => e.Title);
                        break;
                    case "titledesc":
                        OrderByDescending = query => query.OrderByDescending(e => e.Title);
                        break;
                    case "grade":
                        OrderBy = query => query.OrderBy(e => e.Grade);
                        break;
                    case "gradedesc":
                        OrderByDescending = query => query.OrderByDescending(e => e.Grade);
                        break;  
                    case "year":
                        OrderBy = query => query.OrderBy(e => e.Year);
                        break;
                    case "yeardesc":
                        OrderByDescending = query => query.OrderByDescending(e => e.Year);
                        break;
                    case "examcategory":
                        OrderBy = query => query.OrderBy(e => e.ExamCategory.Name);
                        break;
                    case "examcategorydesc":
                        OrderByDescending = query => query.OrderByDescending(e => e.ExamCategory.Name);
                        break;
                    default:
                        OrderByDescending = query => query.OrderByDescending(e => e.CreatedAt);
                        break;
                }
            }
            else
            {
                OrderByDescending = query => query.OrderByDescending(e => e.CreatedAt);
            }

            Includes.Add(e => e.ExamCategory);
            Includes.Add(e => e.User);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
