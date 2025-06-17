using MediatR;
using PhyGen.Application.Chapters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Queries
{
    public class GetChaptersBySubjectBookIdQuery : IRequest<List<ChapterResponse>>
    {
        public Guid SubjectBookId { get; set; }
        public GetChaptersBySubjectBookIdQuery(Guid subjectBookId)
        {
            SubjectBookId = subjectBookId;
        }
    }
}
