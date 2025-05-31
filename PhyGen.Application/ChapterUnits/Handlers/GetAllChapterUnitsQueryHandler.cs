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
    public class GetAllChapterUnitsQueryHandler : IRequestHandler<GetAllChapterUnitsQuery, List<ChapterUnitResponse>>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        private readonly IUserRepository _userRepository;
        public GetAllChapterUnitsQueryHandler(IChapterUnitRepository chapterUnitRepository, IUserRepository userRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
            _userRepository = userRepository;
        }
        public async Task<List<ChapterUnitResponse>> Handle(GetAllChapterUnitsQuery request, CancellationToken cancellationToken)
        {
            var chapterUnits = await _chapterUnitRepository.GetAllAsync();

            var filteredChapterUnits = chapterUnits.Where(cu => cu.DeletedBy == null).ToList();

            var chapterUnitResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterUnitResponse>>(filteredChapterUnits);
            
            return chapterUnitResponses;
        }
    }
}
