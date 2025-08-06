using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs.Topic
{
    public class TopicSpecParam : BaseSpecParam
    {
        public Guid ChapterId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }
}
