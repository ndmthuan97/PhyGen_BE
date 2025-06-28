using MediatR;
using PhyGen.Application.ContentFlows.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Queries
{
    public class GetContentFlowsByCurriculumIdAndSubjectIdQuery : IRequest<List<ContentFlowResponse>>
    {
        public Guid CurriculumId { get; set; }
        public Guid SubjectId { get; set; }
        public GetContentFlowsByCurriculumIdAndSubjectIdQuery(Guid curriculumId, Guid subjectId)
        {
            CurriculumId = curriculumId;
            SubjectId = subjectId;
        }
    }
}
