using MediatR;
using PhyGen.Application.Sections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Queries
{
    public class GetSectionsByExamIdQuery : IRequest<List<SectionResponse>>
    {
        public Guid ExamId { get; set; }
        public GetSectionsByExamIdQuery(Guid examId)
        {
            ExamId = examId;
        }
    }
}
