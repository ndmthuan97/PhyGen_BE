using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Responses
{
    public class ExamQuestionResponse
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
