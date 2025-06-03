using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PhyGen.Application.Questions.Commands
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public Guid QuestionId { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? Type { get; set; }

        public string? Level { get; set; }

        //public string? Image { get; set; }

        public Guid ChapterId { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
