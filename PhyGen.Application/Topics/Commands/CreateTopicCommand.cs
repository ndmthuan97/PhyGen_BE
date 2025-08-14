using MediatR;
using PhyGen.Application.Topics.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Commands
{
    public class CreateTopicCommand : IRequest<TopicResponse>
    {
        public Guid ChapterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TopicCode { get; set; } = string.Empty;
    }
}
