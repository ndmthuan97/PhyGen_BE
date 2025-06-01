using MediatR;
using PhyGen.Application.Books.Queries;
using PhyGen.Application.Books.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Handlers
{
    public class GetBooksByBookSeriesIdQueryHandler : IRequestHandler<GetBooksByBookSeriesIdQuery, List<BookResponse>>
    {
        private readonly IBookRepository _bookRepository;

        public GetBooksByBookSeriesIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookResponse>> Handle(GetBooksByBookSeriesIdQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetBooksByBookSeriesIdAsync(request.BookSeriesId);

            var filteredBooks = books .Where(b => !b.DeletedAt.HasValue).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<BookResponse>>(filteredBooks);
        }
    }
}
