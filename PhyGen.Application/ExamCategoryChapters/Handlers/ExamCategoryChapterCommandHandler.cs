using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Handlers
{
    public class CreateExamCategoryChapterCommandHandler : IRequestHandler<CreateExamCategoryChapterCommand, Guid>
    {
        private readonly IExamCategoryChapterRepository _examCategoryChapterRepository;
        //private readonly IExamCategoryRepository _examCategoryRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateExamCategoryChapterCommandHandler(
            IExamCategoryChapterRepository examCategoryChapterRepository,
            //IExamCategoryRepository examCategoryRepository,
            IChapterRepository chapterRepository)
        {
            _examCategoryChapterRepository = examCategoryChapterRepository;
            //_examCategoryRepository = examCategoryRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Guid> Handle(CreateExamCategoryChapterCommand request, CancellationToken cancellationToken)
        {
            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();
      
            //if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
            //    throw new ExamCategoryNotFoundException();
 
            var examCategoryChapter = new ExamCategoryChapter
            {
                ExamCategoryId = request.ExamCategoryId,
                ChapterId = request.ChapterId
            };

            await _examCategoryChapterRepository.AddAsync(examCategoryChapter);

            return examCategoryChapter.Id;
        }
    }
}