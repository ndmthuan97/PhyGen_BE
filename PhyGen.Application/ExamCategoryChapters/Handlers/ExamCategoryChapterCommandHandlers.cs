using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Application.ExamCategoryChapters.Exceptions;
using PhyGen.Application.ExamCategoryChapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Handlers
{
    public class CreateExamCategoryChapterCommandHandler : IRequestHandler<CreateExamCategoryChapterCommand, ExamCategoryChapterResponse>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateExamCategoryChapterCommandHandler(
            IExamCategoryChapterRepository examCategoryChapterRepository,
            IExamCategoryRepository examCategoryRepository,
            IChapterRepository chapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
            _examCategoryRepository = examCategoryRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<ExamCategoryChapterResponse> Handle(CreateExamCategoryChapterCommand request, CancellationToken cancellationToken)
        {
            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();


            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            if (await _examCategoryChapterRepository.GetByExamCategoryIdAndChapterIdAsync(request.ExamCategoryId, request.ChapterId) != null)
                throw new ExamCategoryChapterAlreadyExistException();

            var examCategoryChapter = new ExamCategoryChapter
            {
                ExamCategoryId = request.ExamCategoryId,
                ChapterId = request.ChapterId
            };

            await _examCategoryChapterRepository.AddAsync(examCategoryChapter);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryChapterResponse>(examCategoryChapter);
        }
    }

    public class UpdateExamCategoryChapterCommandHandler : IRequestHandler<UpdateExamCategoryChapterCommand, Unit>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly IChapterRepository _chapterRepository;

        public UpdateExamCategoryChapterCommandHandler(
            IExamCategoryChapterRepository examCategoryChapterRepository,
            IExamCategoryRepository examCategoryRepository,
            IChapterRepository chapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
            _examCategoryRepository = examCategoryRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Unit> Handle(UpdateExamCategoryChapterCommand request, CancellationToken cancellationToken)
        {
            var examCategoryChapter = await _examCategoryChapterRepository.GetByIdAsync(request.Id);
            if (examCategoryChapter == null)
                throw new ExamCategoryChapterNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            if (await _examCategoryChapterRepository.GetByExamCategoryIdAndChapterIdAsync(request.ExamCategoryId, request.ChapterId) != null)
                throw new ExamCategoryChapterAlreadyExistException();

            examCategoryChapter.ExamCategoryId = request.ExamCategoryId;
            examCategoryChapter.ChapterId = request.ChapterId;

            await _examCategoryChapterRepository.UpdateAsync(examCategoryChapter);
            return Unit.Value;
        }
    }
}
