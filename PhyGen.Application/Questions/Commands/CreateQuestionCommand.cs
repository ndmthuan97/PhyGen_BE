using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PhyGen.Application.Questions.Commands
{
    public class CreateQuestionCommand : IRequest<Guid>
    {
        public string Content { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        //public string Answer { get; set; } = string.Empty;
    }
}
