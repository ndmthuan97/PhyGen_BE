using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItems.Responses
{
    public class ContentItemResponse
    {
        public Guid Id { get; set; }
        public int ContentFlowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }
}
