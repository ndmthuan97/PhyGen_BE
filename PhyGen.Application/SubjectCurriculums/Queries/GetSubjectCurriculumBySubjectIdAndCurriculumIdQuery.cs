using MediatR;
using PhyGen.Application.SubjectCurriculums.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Queries
{
    public class GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery : IRequest<SubjectCurriculumResponse>
    {
        public Guid SubjectId { get; set; }
        public Guid CurriculumId { get; set; }
        public GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery(Guid subjectId, Guid curriculumId)
        {
            SubjectId = subjectId;
            CurriculumId = curriculumId;
        }
    }
}
