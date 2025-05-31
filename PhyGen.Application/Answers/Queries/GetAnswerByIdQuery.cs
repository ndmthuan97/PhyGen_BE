using MediatR;
using PhyGen.Application.Answers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Queries
{
    public class GetAnswerByIdQuery : IRequest<AnswerResponse>
    {
        public Guid AnswerId { get; set; }
        public GetAnswerByIdQuery(Guid answerId) => AnswerId = answerId;
    }
}
