using MediatR;
using PhyGen.Application.ChapterUnits.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Queries
{
    public class GetChapterUnitByIdQuery : IRequest<ChapterUnitResponse>
    {
        public Guid ChapterUnitId { get; set; }
        public GetChapterUnitByIdQuery(Guid chapterUnitId)
        {
            ChapterUnitId = chapterUnitId;
        }
    }
}
