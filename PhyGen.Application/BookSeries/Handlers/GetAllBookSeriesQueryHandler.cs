using MediatR;
using PhyGen.Application.BookSeries.Queries;
using PhyGen.Application.BookSeries.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Handlers
{
    public class GetAllBookSeriesQueryHandler : IRequestHandler<GetAllBookSeriesQuery, List<BookSeriesResponse>>
    {
        private readonly IBookSeriesRepository _bookSeriesRepository;

        public GetAllBookSeriesQueryHandler(IBookSeriesRepository bookSeriesRepository)
        {
            _bookSeriesRepository = bookSeriesRepository;
        }

        public async Task<List<BookSeriesResponse>> Handle(GetAllBookSeriesQuery request, CancellationToken cancellationToken)
        {
            var bookSeries = await _bookSeriesRepository.GetAllAsync();

            var filteredBookSeries = bookSeries.Where(bs => bs.DeletedAt == null).ToList();

            var bookSeriesResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<BookSeriesResponse>>(filteredBookSeries);

            return bookSeriesResponses;
        }
    }
}
