using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Queries
{
    public class GetGradeByTopicId : IRequest<int?>
    {
        public Guid TopicId { get; set; }
        public GetGradeByTopicId(Guid topicId)
        {
            TopicId = topicId;
        }
    }
}
