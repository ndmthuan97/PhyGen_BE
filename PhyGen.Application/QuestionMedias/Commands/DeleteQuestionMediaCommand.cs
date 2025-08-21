using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Commands
{
    public class DeleteQuestionMediaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteQuestionMediaCommand(Guid id)
        {
            Id = id;
        }
    }
}
