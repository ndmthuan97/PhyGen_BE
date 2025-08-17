using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Dtos
{
    public class ExtractedQuestionDto
    {
        public string Content { get; set; }
        public QuestionType Type { get; set; } 
        public DifficultyLevel Level { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public int Grade { get; set; }
        public string TopicName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string> MediaUrls { get; set; } = new();
    }
}
