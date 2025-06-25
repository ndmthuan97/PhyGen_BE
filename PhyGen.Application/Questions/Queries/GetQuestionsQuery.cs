using MediatR;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Queries
{
    public record GetQuestionsQuery(QuestionSpecParam QuestionSpecParam) : IRequest<Pagination<QuestionResponse>>;
}
