using MediatR;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class DeleteChapterCommandHandlers : IRequestHandler<DeleteChapterCommand, Unit>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;

        public DeleteChapterCommandHandlers(IChapterRepository chapterRepository, IUserRepository userRepository)
        {
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.ChapterId);

            if (chapter == null)
                throw new ChapterNotFoundException();

            if (await _userRepository.GetUserByEmailAsync(request.DeletedBy) == null)
                throw new UserNotFoundException();

            chapter.DeletedBy = request.DeletedBy;
            chapter.DeletedAt = DateTime.UtcNow;

            await _chapterRepository.UpdateAsync(chapter);
            return Unit.Value;
        }
    }
}
