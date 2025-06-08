using MediatR;
using PhyGen.Application.Chapters.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Queries
{
    public class GetChapterByIdQuery : IRequest<ChapterResponse>
    {
        public Guid ChapterId { get; set; }
        public GetChapterByIdQuery(Guid id)
        {
            ChapterId = id;
        }
    }
}
