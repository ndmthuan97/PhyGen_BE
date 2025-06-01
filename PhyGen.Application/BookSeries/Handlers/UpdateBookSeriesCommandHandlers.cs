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
    public class UpdateBookSeriesCommandHandlers : IRequestHandler<UpdateBookSeriesCommand, Unit>
    {
        private readonly IBookSeriesRepository _bookSeriesRepository;
        private readonly IUserRepository _userRepository;

        public UpdateBookSeriesCommandHandlers(IBookSeriesRepository bookSeriesRepository, IUserRepository userRepository)
        {
            _bookSeriesRepository = bookSeriesRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateBookSeriesCommand request, CancellationToken cancellationToken)
        {
            var bookSeries = await _bookSeriesRepository.GetByIdAsync(request.Id) ?? throw new BookSeriesNotFoundException();

            if (await _bookSeriesRepository.GetBookSeriesByNameAsync(request.Name) != null)
                throw new BookSeriesSameNameException();

            if (await _userRepository.GetUserByEmailAsync(request.UpdatedBy) == null)
                throw new UserNotFoundException();

            bookSeries.Name = request.Name;
            bookSeries.UpdatedBy = request.UpdatedBy;
            bookSeries.UpdatedAt = DateTime.UtcNow;

            await _bookSeriesRepository.UpdateAsync(bookSeries);
            return Unit.Value;
        }
    }
}
