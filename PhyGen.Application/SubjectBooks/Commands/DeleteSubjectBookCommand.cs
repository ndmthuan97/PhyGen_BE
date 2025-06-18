using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Commands
{
    public class DeleteSubjectBookCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
