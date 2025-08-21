using MediatR;
using PhyGen.Application.Topics.Responses;
using PhyGen.Domain.Specs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Queries
{
    public record GetTopicsByGradeQuery(TopicByGradeSpecParam TopicByGradeSpecParam) : IRequest<List<TopicResponse>>;
}
