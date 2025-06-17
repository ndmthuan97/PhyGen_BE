using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.ExamCategoryChapters.Exceptions;
using PhyGen.Application.ExamCategoryChapters.Queries;
using PhyGen.Application.ExamCategoryChapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Handlers
{
    public class GetExamCategoryChapterByIdQueryHandler : IRequestHandler<GetExamCategoryChapterByIdQuery, ExamCategoryChapterResponse>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;

        public GetExamCategoryChapterByIdQueryHandler(IExamCategoryChapterRepository examCategoryChapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
        }

        public async Task<ExamCategoryChapterResponse> Handle(GetExamCategoryChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var examCategoryChapter = await _examCategoryChapterRepository.GetByIdAsync(request.Id) ?? throw new ExamCategoryChapterNotFoundException();
            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryChapterResponse>(examCategoryChapter);
        }
    }

    public class GetExamCategoryChaptersByExamCategoryIdAndChapterIdQueryHandler : IRequestHandler<GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery, ExamCategoryChapterResponse>
    { 
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly IChapterRepository _chapterRepository;

        public GetExamCategoryChaptersByExamCategoryIdAndChapterIdQueryHandler(
            IExamCategoryChapterRepository examCategoryChapterRepository,
            IExamCategoryRepository examCategoryRepository,
            IChapterRepository chapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
            _examCategoryRepository = examCategoryRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<ExamCategoryChapterResponse> Handle(GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery request, CancellationToken cancellationToken)
        {
            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            var examCategoryChapters = await _examCategoryChapterRepository.GetByExamCategoryIdAndChapterIdAsync(request.ExamCategoryId, request.ChapterId)
                ?? throw new ExamCategoryChapterNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryChapterResponse>(examCategoryChapters);
        }
    }
}
