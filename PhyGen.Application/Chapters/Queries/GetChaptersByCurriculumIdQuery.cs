using MediatR;
using PhyGen.Application.Chapters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Queries
{
    public class GetChaptersByCurriculumIdQuery : IRequest<List<ChapterResponse>>
    {
        public Guid CurriculumId { get; set; }
        public GetChaptersByCurriculumIdQuery(Guid curriculumId)
        {
            CurriculumId = curriculumId;
        }
    }
}
