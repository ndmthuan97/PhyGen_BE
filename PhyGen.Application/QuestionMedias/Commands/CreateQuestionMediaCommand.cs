using MediatR;
using PhyGen.Application.QuestionMedias.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Commands
{
    public class CreateQuestionMediaCommand : IRequest<QuestionMediaResponse>
    {
        public Guid QuestionId { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
