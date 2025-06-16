using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Commands
{
    public class CreateExamQuestionCommand : IRequest<Guid>
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
