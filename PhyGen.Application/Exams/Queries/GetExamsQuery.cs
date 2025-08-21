using MediatR;
using PhyGen.Application.Exams.Responses;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Queries
{
    public record GetExamsQuery(ExamSpecParam ExamSpecParam) : IRequest<Pagination<ExamResponse>>;
}
