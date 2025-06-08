using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Response;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class GetAllChaptersQueryHandler : IRequestHandler<GetAllChaptersQuery, List<ChapterResponse>>
    {
        private readonly IChapterRepository _chapterRepository;

        public GetAllChaptersQueryHandler(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<List<ChapterResponse>> Handle(GetAllChaptersQuery request, CancellationToken cancellationToken)
        {
            var chapters = await _chapterRepository.GetAllAsync();

            var chapterResponses = chapters.Where(c => c.DeletedBy == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterResponse>>(chapterResponses);
        }
    }

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

            if (chapter.DeletedBy != null)
                throw new ChapterNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ChapterResponse>(chapter);
        }
    }
}
