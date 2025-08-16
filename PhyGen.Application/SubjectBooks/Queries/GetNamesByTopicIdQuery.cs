using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Queries
{
    public class GetNamesByTopicIdQuery : IRequest<object>
    {
        public Guid TopicId { get; set; }
        public GetNamesByTopicIdQuery(Guid topicId)
        {
            TopicId = topicId;
        }
    }
}
