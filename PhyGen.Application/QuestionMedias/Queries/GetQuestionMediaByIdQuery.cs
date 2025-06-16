using MediatR;
using PhyGen.Application.QuestionMedias.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Queries
{
    public class GetQuestionMediaByIdQuery : IRequest<QuestionMediaResponse>
    {
        public Guid QuestionMediaId { get; set; }
        public GetQuestionMediaByIdQuery(Guid id)
        {
            QuestionMediaId = id;
        }
    }
}
