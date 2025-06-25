using MediatR;
using PhyGen.Application.QuestionSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Queries
{
    public class GetQuestionSectionsByQuestionIdAndSectionIdQuery : IRequest<QuestionSectionResponse>
    {
        public Guid QuestionId { get; set; }
        public Guid SectionId { get; set; }
        public GetQuestionSectionsByQuestionIdAndSectionIdQuery(Guid questionId, Guid sectionId)
        {
            QuestionId = questionId;
            SectionId = sectionId;
        }
    }
}
