using MediatR;
using PhyGen.Application.BookDetails.Commands;
using PhyGen.Application.Exceptions.BookDetails;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Handlers
{
    public class UpdateBookDetailCommandHandler : IRequestHandler<UpdateBookDetailCommand, Unit>
    {
        private readonly IBookDetailRepository _bookDetailRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IChapterRepository _chapterRepository;

        public UpdateBookDetailCommandHandler(
            IBookDetailRepository bookDetailRepository,
            IBookRepository bookRepository,
            IChapterRepository chapterRepository)
        {
            _bookDetailRepository = bookDetailRepository;
            _bookRepository = bookRepository;
            _chapterRepository = chapterRepository;
        }
        public async Task<Unit> Handle(UpdateBookDetailCommand request, CancellationToken cancellationToken)
        {
            var bookDetail = await _bookDetailRepository.GetByIdAsync(request.Id) ?? throw new BookDetailNotFoundException();

            if (await _bookRepository.GetByIdAsync(request.BookId) == null)
                throw new BookNotFoundException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            bookDetail.BookId = request.BookId;
            bookDetail.ChapterId = request.ChapterId;
            bookDetail.OrderNo = request.OrderNo;

            await _bookDetailRepository.UpdateAsync(bookDetail);
            return Unit.Value;
        }
    }
}
