using MediatR;
using PhyGen.Application.ChapterUnits.Exceptions;
using PhyGen.Application.ChapterUnits.Queries;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
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

            var chapterUnitResponses = chapterUnits.Where(cu => cu.DeletedBy == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterUnitResponse>>(chapterUnitResponses);
        }
    }

    public class GetChapterUnitByIdQueryHandler : IRequestHandler<GetChapterUnitByIdQuery, ChapterUnitResponse>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        public GetChapterUnitByIdQueryHandler(IChapterUnitRepository chapterUnitRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
        }
        public async Task<ChapterUnitResponse> Handle(GetChapterUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var chapterUnit = await _chapterUnitRepository.GetByIdAsync(request.ChapterUnitId) ?? throw new ChapterUnitNotFoundException();

            if (chapterUnit.DeletedBy != null) throw new ChapterUnitNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ChapterUnitResponse>(chapterUnit);
        }
    }

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

            var chapterUnitResponses = chapterUnits.Where(cu => cu.DeletedAt == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterUnitResponse>>(chapterUnitResponses);
        }
    }
}
