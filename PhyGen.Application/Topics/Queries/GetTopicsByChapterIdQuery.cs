using MediatR;
using PhyGen.Application.Topics.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Queries
{
    public class GetTopicsByChapterIdQuery : IRequest<List<TopicResponse>>
    {
        public Guid ChapterId { get; set; }
        public GetTopicsByChapterIdQuery(Guid chapterId)
        {
            ChapterId = chapterId;
        }
    }
}
