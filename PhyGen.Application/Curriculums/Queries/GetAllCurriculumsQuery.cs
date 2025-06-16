using MediatR;
using PhyGen.Application.Curriculums.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Queries
{
    public class GetAllCurriculumsQuery : IRequest<List<CurriculumResponse>>
    {
    }
}
