using MediatR;
using PhyGen.Application.SubjectBooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Commands
{
    public class CreateSubjectBookCommand : IRequest<SubjectBookResponse>
    {
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}
