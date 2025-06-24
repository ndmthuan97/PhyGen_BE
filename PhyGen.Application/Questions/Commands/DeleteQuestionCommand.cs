using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Commands
{
    public class DeleteQuestionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
