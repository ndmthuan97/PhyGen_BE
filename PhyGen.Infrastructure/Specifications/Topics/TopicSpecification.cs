using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Specifications.Topics
{
    public class TopicSpecification : ISpecification<Topic>
    {
        public Expression<Func<Topic, bool>> Criteria { get; private set; }

        public Func<IQueryable<Topic>, IOrderedQueryable<Topic>>? OrderBy { get; private set; }

        public Func<IQueryable<Topic>, IOrderedQueryable<Topic>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Topic, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Topic>, IQueryable<Topic>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public TopicSpecification(TopicSpecParam param)
        {
            Criteria = topic =>
                topic.ChapterId == param.ChapterId &&
                (string.IsNullOrEmpty(param.Search) || topic.Name.ToLower().Contains(param.Search.ToLower()))
                && !topic.DeletedAt.HasValue;
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    //case "orderno":
                    //    OrderBy = query => query.OrderBy(t => t.OrderNo);
                    //    break;
                    //case "ordernodesc":
                    //    OrderByDescending = query => query.OrderByDescending(t => t.OrderNo);
                    //    break;
                    //default:
                    //    OrderBy = query => query.OrderBy(t => t.OrderNo);
                    //    break;
                }
            }
            else
            {
                //OrderBy = query => query.OrderBy(t => t.OrderNo);
            }
            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
