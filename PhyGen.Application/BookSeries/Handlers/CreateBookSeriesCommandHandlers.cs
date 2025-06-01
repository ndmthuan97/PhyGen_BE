using MediatR;
using PhyGen.Application.BookSeries.Commands;
using PhyGen.Application.Exceptions.BookSeries;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Handlers
{
    public class CreateBookSeriesCommandHandlers : IRequestHandler<CreateBookSeriesCommand, Guid>
    {
        private readonly IBookSeriesRepository _bookSeriesRepository;
        private readonly IUserRepository _userRepository;

        public CreateBookSeriesCommandHandlers(IBookSeriesRepository bookSeriesRepository, IUserRepository userRepository)
        {
            _bookSeriesRepository = bookSeriesRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateBookSeriesCommand request, CancellationToken cancellationToken)
        {
            if (await _bookSeriesRepository.GetBookSeriesByNameAsync(request.Name) != null) 
                throw new BookSeriesSameNameException();

            if (await _userRepository.GetUserByEmailAsync(request.CreatedBy) == null)
                throw new UserNotFoundException();

            var bookSeries = new Domain.Entities.BookSeries
            {
                Name = request.Name,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            bookSeries = await _bookSeriesRepository.AddAsync(bookSeries);
            return bookSeries.Id;
        }
    }
}
