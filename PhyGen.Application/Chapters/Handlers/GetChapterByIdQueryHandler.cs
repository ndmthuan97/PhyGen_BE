using MediatR;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
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
}
