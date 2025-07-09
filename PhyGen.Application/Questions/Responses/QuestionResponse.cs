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
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public string? CorrectAnswer { get; set; }
    }
}
