using MediatR;
using PhyGen.Application.Questions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Queries
{
    public class GetQuestionByIdQuery : IRequest<QuestionResponse>
    {
        public Guid QuestionId { get; set; }
        public GetQuestionByIdQuery(Guid id)
        {
            QuestionId = id;
        }
    }
}
