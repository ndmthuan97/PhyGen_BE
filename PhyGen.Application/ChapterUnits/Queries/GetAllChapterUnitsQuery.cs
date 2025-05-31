using MediatR;
using PhyGen.Application.ChapterUnits.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Queries
{
    public class GetAllChapterUnitsQuery : IRequest<List<ChapterUnitResponse>>
    {
    }
}
