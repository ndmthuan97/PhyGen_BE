using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Commands
{
    public class UpdateAnswerCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
