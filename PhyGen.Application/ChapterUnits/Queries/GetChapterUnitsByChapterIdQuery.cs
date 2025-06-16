using MediatR;
using PhyGen.Application.ChapterUnits.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Queries
{
    public class GetChapterUnitsByChapterIdQuery : IRequest<List<ChapterUnitResponse>>
    {
        public Guid ChapterId { get; set; }
        public GetChapterUnitsByChapterIdQuery(Guid chapterId)
        {
            ChapterId = chapterId;
        }
    }
}
