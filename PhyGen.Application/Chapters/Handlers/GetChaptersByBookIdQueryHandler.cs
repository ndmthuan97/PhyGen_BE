using MediatR;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class GetChaptersByBookIdQueryHandler : IRequestHandler<GetChaptersByBookIdQuery, List<ChapterResponse>>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IBookRepository _bookRepository;

        public GetChaptersByBookIdQueryHandler(IChapterRepository chapterRepository, IBookRepository bookRepository)
        {
            _chapterRepository = chapterRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<ChapterResponse>> Handle(GetChaptersByBookIdQuery request, CancellationToken cancellationToken)
        {
            if (await _bookRepository.GetByIdAsync(request.BookId) == null)
                throw new BookNotFoundException();

            var chapters = await _chapterRepository.GetChaptersByBookIdAsync(request.BookId);
            var filteredChapters = chapters.Where(c => c.DeletedBy == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterResponse>>(filteredChapters);
        }
    }
}
