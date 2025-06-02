using MediatR;
using PhyGen.Application.BookDetails.Queries;
using PhyGen.Application.BookDetails.Responses;
using PhyGen.Application.Exceptions.BookDetails;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Handlers
{
    public class GetBookDetailByBookIdAndChapterIdQueryHandler : IRequestHandler<GetBookDetailByBookIdAndChapterIdQuery, BookDetailResponse>
    {
        private readonly IBookDetailRepository _bookDetailRepository;

        public GetBookDetailByBookIdAndChapterIdQueryHandler(IBookDetailRepository bookDetailRepository)
        {
            _bookDetailRepository = bookDetailRepository;
        }

        public async Task<BookDetailResponse> Handle(GetBookDetailByBookIdAndChapterIdQuery request, CancellationToken cancellationToken)
        {
            var bookDetail = await _bookDetailRepository.GetBookDetailByBookIdAndChapterIdAsync(request.BookId, request.ChapterId)
                ?? throw new BookDetailNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<BookDetailResponse>(bookDetail);
        }
    }
}
