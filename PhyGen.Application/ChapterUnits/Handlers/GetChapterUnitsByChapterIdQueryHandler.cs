using MediatR;
using PhyGen.Application.ChapterUnits.Queries;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Handlers
{
    public class GetChapterUnitsByChapterIdQueryHandler : IRequestHandler<GetChapterUnitsByChapterIdQuery, List<ChapterUnitResponse>>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        public GetChapterUnitsByChapterIdQueryHandler(IChapterUnitRepository chapterUnitRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
        }
        public async Task<List<ChapterUnitResponse>> Handle(GetChapterUnitsByChapterIdQuery request, CancellationToken cancellationToken)
        {
            var chapterUnits = await _chapterUnitRepository.GetChapterUnitsByChapterIdAsync(request.ChapterId);

            var filteredChapterUnits = chapterUnits.Where(cu => cu.DeletedAt == null).ToList();

            var chapterUnitResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterUnitResponse>>(filteredChapterUnits);
            
            return chapterUnitResponses;
        }
    }
}
