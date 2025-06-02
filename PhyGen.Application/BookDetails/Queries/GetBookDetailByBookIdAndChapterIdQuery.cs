using MediatR;
using PhyGen.Application.BookDetails.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Queries
{
    public class GetBookDetailByBookIdAndChapterIdQuery : IRequest<BookDetailResponse>
    {
        public Guid BookId { get; set; }
        public Guid ChapterId { get; set; }
        public GetBookDetailByBookIdAndChapterIdQuery(Guid bookId, Guid chapterId)
        {
            BookId = bookId;
            ChapterId = chapterId;
        }
    }
}
