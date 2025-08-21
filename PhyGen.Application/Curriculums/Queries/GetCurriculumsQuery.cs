using MediatR;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Queries
{
    public record GetCurriculumsQuery(CurriculumSpecParam CurriculumSpecParam) : IRequest<Pagination<CurriculumResponse>>;
}
