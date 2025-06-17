using MediatR;
using PhyGen.Application.SubjectBooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Queries
{
    public class GetSubjectBooksBySubjectIdQuery : IRequest<List<SubjectBookResponse>>
    {
        public Guid SubjectId { get; set; }
        public GetSubjectBooksBySubjectIdQuery(Guid subjectId)
        {
            SubjectId = subjectId;
        }
    }
}
