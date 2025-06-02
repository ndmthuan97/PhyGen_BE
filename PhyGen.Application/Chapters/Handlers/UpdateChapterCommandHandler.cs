using MediatR;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, Unit>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IBookRepository _bookRepository;

        public UpdateChapterCommandHandler(IChapterRepository chapterRepository, IUserRepository userRepository,
            ICurriculumRepository curriculumRepository, IBookRepository bookRepository)
        {
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
            _curriculumRepository = curriculumRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Unit> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.ChapterId);

            if (await _userRepository.GetUserByEmailAsync(request.UpdatedBy) == null)
                throw new UserNotFoundException();

            if (await _chapterRepository.GetChapterByNameAsync(request.Name) != null)
                throw new ChapterSameNameException();

            chapter.Name = request.Name;
            chapter.CurriculumId = request.CurriculumId;
            chapter.OrderNo = request.OrderNo;
            chapter.UpdatedBy = request.UpdatedBy;
            chapter.UpdatedAt = DateTime.UtcNow;

            await _chapterRepository.UpdateAsync(chapter);
            return Unit.Value;
        }
    }
}
