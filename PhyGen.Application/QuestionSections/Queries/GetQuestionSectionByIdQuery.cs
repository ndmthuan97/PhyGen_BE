using MediatR;
using PhyGen.Application.QuestionSections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Queries
{
    public class GetQuestionSectionByIdQuery : IRequest<QuestionSectionResponse>
    {
        public Guid Id { get; set; }
        public GetQuestionSectionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
