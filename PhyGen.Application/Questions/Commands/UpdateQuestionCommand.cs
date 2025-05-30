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
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? UpdatedBy { get; set; }

        //public string Answer { get; set; } = string.Empty;
    }
}
