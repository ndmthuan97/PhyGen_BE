using MediatR;
using PhyGen.Application.Chapters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Queries
{
    public class GetChaptersByBookIdQuery : IRequest<List<ChapterResponse>>
    {
        public Guid BookId { get; set; }
        public GetChaptersByBookIdQuery(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
