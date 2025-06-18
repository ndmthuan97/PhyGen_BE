using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Exceptions;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, ChapterResponse>
    {
        private readonly IChapterRepository _chapterRepository;

        public GetChapterByIdQueryHandler(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<ChapterResponse> Handle(GetChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.ChapterId) ?? throw new ChapterNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ChapterResponse>(chapter);
        }
    }

    public class GetChaptersBySubjectBookIdQueryHandler : IRequestHandler<GetChaptersBySubjectBookIdQuery, Pagination<ChapterResponse>>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ISubjectBookRepository _subjectBookRepository;

        public GetChaptersBySubjectBookIdQueryHandler(IChapterRepository chapterRepository, ISubjectBookRepository subjectBookRepository)
        {
            _chapterRepository = chapterRepository;
            _subjectBookRepository = subjectBookRepository;
        }

        public async Task<Pagination<ChapterResponse>> Handle(GetChaptersBySubjectBookIdQuery request, CancellationToken cancellationToken)
        {
            var chapters = await _chapterRepository.GetChaptersBySubjectBookAsync(request.ChapterSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<ChapterResponse>>(chapters);
        }
    }
}
