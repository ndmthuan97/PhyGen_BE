using MediatR;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Queries
{
    public record GetQuestionByIdQuery : IRequest<QuestionResponse>
    {
        public Guid QuestionId { get; set; }
        public GetQuestionByIdQuery(Guid questionId)
        {
            QuestionId = questionId;
        }
    }
}
