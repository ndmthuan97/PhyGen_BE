using MediatR;
using PhyGen.Application.BookSeries.Queries;
using PhyGen.Application.BookSeries.Responses;
using PhyGen.Application.Exceptions.BookSeries;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Handlers
{
    public class GetBookSeriesByIdQueryHandler : IRequestHandler<GetBookSeriesByIdQuery, BookSeriesResponse>
    {
        private readonly IBookSeriesRepository _bookSeriesRepository;

        public GetBookSeriesByIdQueryHandler(IBookSeriesRepository bookSeriesRepository)
        {
            _bookSeriesRepository = bookSeriesRepository;
        }

        public async Task<BookSeriesResponse> Handle(GetBookSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var bookSeries = await _bookSeriesRepository.GetByIdAsync(request.Id) ?? throw new BookSeriesNotFoundException();
            return AppMapper<CoreMappingProfile>.Mapper.Map<BookSeriesResponse>(bookSeries);
        }
    }
}
