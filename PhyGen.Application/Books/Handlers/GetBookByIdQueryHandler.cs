using MediatR;
using PhyGen.Application.Books.Queries;
using PhyGen.Application.Books.Responses;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookResponse> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId) ?? throw new BookNotFoundException();

            if (book.DeletedAt.HasValue)  throw new BookNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<BookResponse>(book);
        }
    }
}
