using MediatR;
using PhyGen.Application.Subjects.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Subjects.Queries
{
    public class GetSubjectByIdQuery : IRequest<SubjectResponse>
    {
        public Guid SubjectId { get; set; }
        public GetSubjectByIdQuery(Guid subjectId)
        {
            SubjectId = subjectId;
        }
    }
}
