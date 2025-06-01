using MediatR;
using PhyGen.Application.Books.Commands;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.BookSeries;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Handlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookSeriesRepository _bookSeriesRepository;
        private readonly IUserRepository _userRepository;

        public UpdateBookCommandHandler( IBookRepository bookRepository, IBookSeriesRepository bookSeriesRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _bookSeriesRepository = bookSeriesRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId) ?? throw new BookNotFoundException();

            if (request.SeriesId != null)
            {
                if (await _bookSeriesRepository.GetByIdAsync(request.SeriesId.Value) == null)
                    throw new BookSeriesNotFoundException();
            }

            if (await _userRepository.GetUserByEmailAsync(request.UpdatedBy) == null)
                throw new UserNotFoundException();

            book.Name = request.Name;
            book.SeriesId = request.SeriesId;
            book.Author = request.Author;
            book.PublicationYear = request.PublicationYear;
            book.UpdatedBy = request.UpdatedBy;
            book.UpdatedAt = DateTime.UtcNow;

            await _bookRepository.UpdateAsync(book);
            return Unit.Value;
        }
    }
}
