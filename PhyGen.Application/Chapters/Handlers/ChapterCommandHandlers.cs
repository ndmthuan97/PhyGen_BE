using MediatR;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class CreateChapterCommandHandlers : IRequestHandler<CreateChapterCommand, ChapterResponse>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ISubjectBookRepository _subjectBookRepository;

        public CreateChapterCommandHandlers(IChapterRepository chapterRepository, ISubjectBookRepository subjectBookRepository)
        {
            _chapterRepository = chapterRepository;
            _subjectBookRepository = subjectBookRepository;
        }

        public async Task<ChapterResponse> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectBookRepository.GetByIdAsync(request.SubjectBookId) == null)
                throw new SubjectBookNotFoundException();

            if (await _chapterRepository.GetChapterBySubjectBookIdAndNameAsync(request.SubjectBookId, request.Name) != null)
                throw new ChapterSameNameException();

            var chapter = new Chapter
            {
                SubjectBookId = request.SubjectBookId,
                Name = request.Name,
                OrderNo = request.OrderNo
            };

            await _chapterRepository.AddAsync(chapter);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ChapterResponse>(chapter);
        }
    }
    public class UpdateChapterCommandHandlers : IRequestHandler<UpdateChapterCommand, Unit>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ISubjectBookRepository _subjectBookRepository;
        public UpdateChapterCommandHandlers(IChapterRepository chapterRepository, ISubjectBookRepository subjectBookRepository)
        {
            _chapterRepository = chapterRepository;
            _subjectBookRepository = subjectBookRepository;
        }

        public async Task<Unit> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.Id) ?? throw new ChapterNotFoundException();

            if (await _subjectBookRepository.GetByIdAsync(request.SubjectBookId) == null)
                throw new SubjectBookNotFoundException();

            if (await _chapterRepository.GetChapterBySubjectBookIdAndNameAsync(request.SubjectBookId, request.Name) != null &&
                chapter.Name != request.Name)
                throw new ChapterSameNameException();

            chapter.SubjectBookId = request.SubjectBookId;
            chapter.Name = request.Name;
            chapter.OrderNo = request.OrderNo;

            await _chapterRepository.UpdateAsync(chapter);
            return Unit.Value;
        }
    }
}
