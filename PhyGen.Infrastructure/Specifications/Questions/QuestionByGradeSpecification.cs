using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications.Questions
{
    public class QuestionByGradeSpecification : ISpecification<Question>
    {
        public Expression<Func<Question, bool>> Criteria { get; private set; }

        public Func<IQueryable<Question>, IOrderedQueryable<Question>>? OrderBy { get; private set; }

        public Func<IQueryable<Question>, IOrderedQueryable<Question>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Question, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Question>, IQueryable<Question>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public QuestionByGradeSpecification(QuestionByGradeSpecParam param)
        {
            Criteria = question =>
                (string.IsNullOrEmpty(param.Search) || question.Content.ToLower().Contains(param.Search.ToLower())) &&
                (question.Grade == param.Grade) &&
                (param.CreatedByList == null || param.CreatedByList.Contains(question.CreatedBy));

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "createdat":
                        OrderBy = query => query.OrderBy(q => q.CreatedAt);
                        break;
                    case "createdatdesc":
                        OrderByDescending = query => query.OrderByDescending(q => q.CreatedAt);
                        break;
                    default:
                        OrderByDescending = query => query.OrderByDescending(q => q.CreatedAt);
                        break;
                }
            }

            Includes.Add(q => q.Topic);
            Includes.Add(q => q.Topic.Chapter);
            Includes.Add(q => q.Topic.Chapter.SubjectBook);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
