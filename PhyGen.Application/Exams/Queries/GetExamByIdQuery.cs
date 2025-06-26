using MediatR;
using PhyGen.Application.Exams.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Queries
{
    public class GetExamByIdQuery : IRequest<ExamResponse>
    {
        public Guid Id { get; set; }
        public GetExamByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
