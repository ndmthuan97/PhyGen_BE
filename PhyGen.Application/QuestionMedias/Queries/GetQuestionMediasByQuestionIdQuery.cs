using MediatR;
using PhyGen.Application.QuestionMedias.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Queries
{
    public class GetQuestionMediasByQuestionIdQuery : IRequest<List<QuestionMediaResponse>>
    {
        public Guid QuestionId { get; set; }
        public GetQuestionMediasByQuestionIdQuery(Guid questionId)
        {
            QuestionId = questionId;
        }
    }
}
