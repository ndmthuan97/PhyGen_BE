using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PhyGen.Application.Questions.Commands
{
    public class DeleteQuestionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteQuestionCommand(Guid id)
        {
            Id = id;
        }
    }
}
