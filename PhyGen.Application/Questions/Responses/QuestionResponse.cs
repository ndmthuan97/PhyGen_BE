using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Responses
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string Content { get; set; } = string.Empty;
        [JsonIgnore]
        public DifficultyLevel Level { get; set; }
        public string LevelName => Level.ToString();    
        [JsonIgnore]
        public QuestionType Type { get; set; }
        public string TypeName => Type.ToString();
        public string? Image { get; set; }
    }
}
