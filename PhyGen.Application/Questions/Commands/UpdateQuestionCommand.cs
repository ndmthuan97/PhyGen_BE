using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Commands
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Guid ChapterUnitId { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public string? CorrectAnswer { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
