using MediatR;
using PhyGen.Application.Books.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Queries
{
    public class GetBooksByBookSeriesIdQuery : IRequest<List<BookResponse>>
    {
        public Guid BookSeriesId { get; set; }
        public GetBooksByBookSeriesIdQuery(Guid bookSeriesId)
        {
            BookSeriesId = bookSeriesId;
        }
    }
}
