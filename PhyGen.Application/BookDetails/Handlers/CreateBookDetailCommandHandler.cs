using MediatR;
using PhyGen.Application.BookDetails.Commands;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Handlers
{
    public class CreateBookDetailCommandHandler : IRequestHandler<CreateBookDetailCommand, Guid>
    {
        private readonly IBookDetailRepository _bookDetailRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateBookDetailCommandHandler(
            IBookDetailRepository bookDetailRepository,
            IBookRepository bookRepository,
            IChapterRepository chapterRepository)
        {
            _bookDetailRepository = bookDetailRepository;
            _bookRepository = bookRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Guid> Handle(CreateBookDetailCommand request, CancellationToken cancellationToken)
        {
            if (await _bookRepository.GetByIdAsync(request.BookId) == null)
            throw new BookNotFoundException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            var bookDetail = new BookDetail
            {
                BookId = request.BookId,
                ChapterId = request.ChapterId,
                OrderNo = request.OrderNo
            };

            var createdBookDetail = await _bookDetailRepository.AddAsync(bookDetail);
            return createdBookDetail.Id;
        }
    }
}
