using MediatR;
using PhyGen.Application.Books.Commands;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.BookSeries;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookSeriesRepository _bookSeriesRepository;
        private readonly IUserRepository _userRepository;

        public CreateBookCommandHandler(IBookRepository bookRepository, IBookSeriesRepository bookSeriesRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _bookSeriesRepository = bookSeriesRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if (request.SeriesId != null)
            {
                if (await _bookSeriesRepository.GetByIdAsync(request.SeriesId.Value) == null)
                    throw new BookSeriesNotFoundException();
            }

            if (await _userRepository.GetUserByEmailAsync(request.CreatedBy) == null)
                throw new UserNotFoundException();

            var book = new Book
            {
                Name = request.Name,
                SeriesId = request.SeriesId,
                Author = request.Author,
                PublicationYear = request.PublicationYear,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            var createdBook = await _bookRepository.AddAsync(book);
            return createdBook.Id;
        }
    }
}
