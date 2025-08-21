using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Commands
{
    public class UpdateQuestionSectionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SectionId { get; set; }
        public double? Score { get; set; }
    }
}
