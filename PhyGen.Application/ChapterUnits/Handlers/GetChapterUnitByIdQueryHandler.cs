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
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;
        public GetChapterUnitByIdQueryHandler(IChapterUnitRepository chapterUnitRepository, IChapterRepository chapterRepository, IUserRepository userRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
        }
        public async Task<ChapterUnitResponse> Handle(GetChapterUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var chapterUnit = await _chapterUnitRepository.GetByIdAsync(request.ChapterUnitId) ?? throw new ChapterUnitNotFoundException();

            var chater = await _chapterRepository.GetByIdAsync(chapterUnit.ChapterId) ?? throw new ChapterNotFoundException();

            if (await _userRepository.GetUserByEmailAsync(chapterUnit.CreatedBy) == null)
                throw new UserNotFoundException();

            if (chapterUnit.DeletedAt.HasValue) throw new ChapterUnitNotFoundException();

            var chapterUnitResponse = AppMapper<CoreMappingProfile>.Mapper.Map<ChapterUnitResponse>(chapterUnit);

            return chapterUnitResponse;
        }
    }
}
