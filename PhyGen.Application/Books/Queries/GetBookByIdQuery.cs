using MediatR;
using PhyGen.Application.Books.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Queries
{
    public class GetBookByIdQuery : IRequest<BookResponse>
    {
        public Guid BookId { get; set; }
        public GetBookByIdQuery(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
