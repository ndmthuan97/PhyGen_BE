using MediatR;
using PhyGen.Application.SubjectCurriculums.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Queries
{
    public class GetSubjectCurriculumByIdQuery : IRequest<SubjectCurriculumResponse>
    {
        public Guid SubjectCurriculumId { get; set; }
        public GetSubjectCurriculumByIdQuery(Guid subjectCurriculumId)
        {
            SubjectCurriculumId = subjectCurriculumId;
        }
    }
}
