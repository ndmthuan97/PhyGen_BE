using MediatR;
using PhyGen.Application.QuestionSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Commands
{
    public class CreateQuestionSectionCommand : IRequest<QuestionSectionResponse>
    {
        public Guid QuestionId { get; set; }
        public Guid SectionId { get; set; }
        public double? Score { get; set; }
    }
}
