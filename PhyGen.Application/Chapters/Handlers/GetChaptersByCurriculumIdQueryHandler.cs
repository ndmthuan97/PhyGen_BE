using MediatR;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class GetChaptersByCurriculumIdQueryHandler : IRequestHandler<GetChaptersByCurriculumIdQuery, List<ChapterResponse>>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurriculumRepository _curriculumRepository;

        public GetChaptersByCurriculumIdQueryHandler(IChapterRepository chapterRepository, ICurriculumRepository curriculumRepository)
        {
            _chapterRepository = chapterRepository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<List<ChapterResponse>> Handle(GetChaptersByCurriculumIdQuery request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            var chapters = await _chapterRepository.GetChaptersByCurriculumIdAsync(request.CurriculumId);
            var filteredChapters = chapters.Where(c => c.DeletedBy == null).ToList();
            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ChapterResponse>>(filteredChapters);
        }
    }
}
