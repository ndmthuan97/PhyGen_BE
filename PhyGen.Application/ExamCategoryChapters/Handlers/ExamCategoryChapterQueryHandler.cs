using MediatR;
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
    public class GetAllExamCategoryChaptersQueryHandler : IRequestHandler<GetAllExamCategoryChaptersQuery, List<ExamCategoryChapterResponse>>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        public GetAllExamCategoryChaptersQueryHandler(IExamCategoryChapterRepository examCategoryChapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
        }
        public async Task<List<ExamCategoryChapterResponse>> Handle(GetAllExamCategoryChaptersQuery request, CancellationToken cancellationToken)
        {
            var examCategoryChapters = await _examCategoryChapterRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ExamCategoryChapterResponse>>(examCategoryChapters);
        }
    }

    public class GetExamCategoryChapterByIdQueryHandler : IRequestHandler<GetExamCategoryChapterByIdQuery, ExamCategoryChapterResponse>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        public GetExamCategoryChapterByIdQueryHandler(IExamCategoryChapterRepository examCategoryChapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
        }
        public async Task<ExamCategoryChapterResponse> Handle(GetExamCategoryChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var examCategoryChapter = await _examCategoryChapterRepository.GetByIdAsync(request.ExamCategoryChapterId)
                ?? throw new ExamCategoryChapterNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryChapterResponse>(examCategoryChapter);
        }
    }
}
