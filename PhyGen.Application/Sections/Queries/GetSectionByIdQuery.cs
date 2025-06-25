using MediatR;
using PhyGen.Application.Sections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Queries
{
    public class GetSectionByIdQuery : IRequest<SectionResponse>
    {
        public Guid Id { get; set; }
        public GetSectionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
