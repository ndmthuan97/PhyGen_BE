using MediatR;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Commands
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public string? Image { get; set; }
    }
}
