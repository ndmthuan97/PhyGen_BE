using MediatR;
using PhyGen.Application.BookSeries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Queries
{
    public class GetBookSeriesByIdQuery : IRequest<BookSeriesResponse>
    {
        public Guid Id { get; set; }
        public GetBookSeriesByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
