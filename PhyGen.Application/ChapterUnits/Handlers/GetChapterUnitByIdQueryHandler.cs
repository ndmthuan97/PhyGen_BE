using MediatR;
using PhyGen.Application.ChapterUnits.Queries;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.ChapterUnits;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Handlers
{
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

            if (chapterUnit.DeletedAt.HasValue) throw new ChapterUnitNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ChapterUnitResponse>(chapterUnit);
        }
    }
}
