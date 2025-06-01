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
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookResponse>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookResponse>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllAsync();

            var filteredBooks = books.Where(b => b.DeletedAt == null).ToList();

            var bookResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<BookResponse>>(filteredBooks);

            return bookResponses;
        }
    }
}
