using MediatR;
using PhyGen.Application.Questions.Responses;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Commands
{
    public class CreateQuestionCommand : IRequest<QuestionResponse>
    {
        public Guid TopicId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public string? Image { get; set; }
    }
}
