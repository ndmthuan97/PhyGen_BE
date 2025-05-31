using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Responses
{
    public class AnswerResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
    }
}
