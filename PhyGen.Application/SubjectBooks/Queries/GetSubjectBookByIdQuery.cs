using MediatR;
using PhyGen.Application.SubjectBooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Queries
{
    public class GetSubjectBookByIdQuery : IRequest<SubjectBookResponse>
    {
        public Guid Id { get; set; }
        public GetSubjectBookByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
