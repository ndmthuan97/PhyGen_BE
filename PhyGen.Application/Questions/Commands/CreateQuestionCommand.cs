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
        public Guid? TopicId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public int Grade { get; set; }

        public StatusQEM Status { get; set; } = StatusQEM.Draft;
        public string? QuestionCode { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }
    }
}
