using MediatR;
using PhyGen.Application.Exams.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Queries
{
    public class GetAllExamsQuery : IRequest<List<ExamResponse>>
    {
    }
}
