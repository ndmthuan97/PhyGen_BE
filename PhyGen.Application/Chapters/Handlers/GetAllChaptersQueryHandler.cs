using MediatR;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
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

            var chapterResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterResponse>>(chapters);

            return chapterResponses;
        }
    }
}
