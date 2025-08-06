using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs.Question
{
    public class QuestionByTopicSpecParam : BaseSpecParam
    {
        public Guid TopicId { get; set; }

        public DifficultyLevel? Level { get; set; }

        public QuestionType? Type { get; set; }

        public List<string>? CreatedByList { get; set; }

        public string? Search { get; set; }

        public string? Sort { get; set; }
    }
}
