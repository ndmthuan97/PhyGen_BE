using MediatR;
using PhyGen.Application.ExamQuestions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Queries
{
    public class GetExamQuestionByIdQuery : IRequest<ExamQuestionResponse>
    {
        public Guid ExamQuestionId { get; set; }
        public GetExamQuestionByIdQuery(Guid id)
        {
            ExamQuestionId = id;
        }
    }
}
