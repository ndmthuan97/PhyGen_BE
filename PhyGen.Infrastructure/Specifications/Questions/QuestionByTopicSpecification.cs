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
    public class QuestionByTopicSpecification : ISpecification<Question>
    {
        public Expression<Func<Question, bool>> Criteria { get; private set; }

        public Func<IQueryable<Question>, IOrderedQueryable<Question>>? OrderBy { get; private set; }

        public Func<IQueryable<Question>, IOrderedQueryable<Question>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Question, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Question>, IQueryable<Question>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public QuestionByTopicSpecification(QuestionByTopicSpecParam param)
        {
            Criteria = question =>
                question.TopicId == param.TopicId &&
                (string.IsNullOrEmpty(param.Search) || question.Content.ToLower().Contains(param.Search.ToLower())) &&
                (param.CreatedByList == null || param.CreatedByList.Contains(question.CreatedBy)) &&
                (!param.Level.HasValue || question.Level == param.Level.Value) &&
                (!param.Type.HasValue || question.Type == param.Type.Value);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "level":
                        OrderBy = query => query.OrderBy(q => q.Level);
                        break;
                    case "leveldesc":
                        OrderByDescending = query => query.OrderByDescending(q => q.Level);
                        break;
                    case "type":
                        OrderBy = query => query.OrderBy(q => q.Type);
                        break;
                    case "typedesc":
                        OrderByDescending = query => query.OrderByDescending(q => q.Type);
                        break;
                    case "createdat":
                        OrderBy = query => query.OrderBy(q => q.CreatedAt);
                        break;
                    case "createdatdesc":
                        OrderByDescending = query => query.OrderByDescending(q => q.CreatedAt);
                        break;
                    case "code":
                        OrderBy = query => query.OrderBy(q => q.QuestionCode);
                        break;
                    case "codedesc":
                        OrderByDescending = query => query.OrderByDescending(q => q.QuestionCode);
                        break;
                    default:
                        OrderByDescending = query => query.OrderByDescending(q => q.QuestionCode);
                        break;
                }
            }

            Includes.Add(q => q.Topic);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
