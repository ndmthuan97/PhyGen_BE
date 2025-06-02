using MediatR;
using PhyGen.Application.BookDetails.Commands;
using PhyGen.Application.Exceptions.BookDetails;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Handlers
{
    public class DeleteBookDetailCommandHandler : IRequestHandler<DeleteBookDetailCommand, Unit>
    {
        private readonly IBookDetailRepository _bookDetailRepository;

        public DeleteBookDetailCommandHandler(IBookDetailRepository bookDetailRepository)
        {
            _bookDetailRepository = bookDetailRepository;
        }

        public async Task<Unit> Handle(DeleteBookDetailCommand request, CancellationToken cancellationToken)
        {
            var bookDetail = await _bookDetailRepository.GetByIdAsync(request.Id) ?? throw new BookDetailNotFoundException();

            await _bookDetailRepository.DeleteAsync(bookDetail);
            return Unit.Value;
        }
    }
}
