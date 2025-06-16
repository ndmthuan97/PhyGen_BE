using MediatR;
using PhyGen.Application.Curriculums.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Queries
{
    public class GetCurriculumByIdQuery : IRequest<CurriculumResponse>
    {
        public Guid CurriculumId { get; set; }

        public GetCurriculumByIdQuery(Guid curriculumId)
        {
            CurriculumId = curriculumId;
        }
    }
}
