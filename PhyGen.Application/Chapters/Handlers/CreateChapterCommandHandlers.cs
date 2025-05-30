using MediatR;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Exceptions.Books;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Handlers
{
    public class CreateChapterCommandHandlers : IRequestHandler<CreateChapterCommand, Guid>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IBookRepository _bookRepository;

        public CreateChapterCommandHandlers(IChapterRepository chapterRepository, IUserRepository userRepository, 
            ICurriculumRepository curriculumRepository, IBookRepository bookRepository)
        {
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
            _curriculumRepository = curriculumRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Guid> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByEmailAsync(request.CreatedBy) == null)
                throw new UserNotFoundException();

            if (await _chapterRepository.GetChapterByTitleAsync(request.Title) != null)
                throw new ChapterSameNameException();

            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            if (await _bookRepository.GetByIdAsync(request.BookId) == null)
                throw new BookNotFoundException();

            var chapter = new Chapter
            {
                Title = request.Title,
                CurriculumId = request.CurriculumId,
                BookId = request.BookId,
                OrderNo = request.OrderNo,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            chapter = await _chapterRepository.AddAsync(chapter);
            return chapter.Id;
        }
    }
}
