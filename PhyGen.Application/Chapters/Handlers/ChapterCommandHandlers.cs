using MediatR;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.SubjectCurriculums.Exceptions;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
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
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;
        private readonly IUserRepository _userRepository;
        public CreateChapterCommandHandlers(IChapterRepository chapterRepository, ISubjectCurriculumRepository subjectCurriculumRepository, IUserRepository userRepository)
        {
            _chapterRepository = chapterRepository;
            _subjectCurriculumRepository = subjectCurriculumRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectCurriculumRepository.GetByIdAsync(request.SubjectCurriculumId) == null)
                throw new SubjectCurriculumNotFoundException();

            if (!Guid.TryParse(request.CreatedBy, out Guid createdByGuid))
                throw new InvalidOperationException("Invalid GUID format for CreatedBy.");

            if (await _userRepository.GetByIdAsync(createdByGuid) == null)
                throw new UserNotFoundException();

            var chapter = new Chapter
            {
                Name = request.Name,
                SubjectCurriculumId = request.SubjectCurriculumId,
                OrderNo = request.OrderNo,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };
            await _chapterRepository.AddAsync(chapter);
            return chapter.Id;
        }
    }

    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, Unit>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;
        private readonly IUserRepository _userRepository;

        public UpdateChapterCommandHandler(IChapterRepository chapterRepository, ISubjectCurriculumRepository subjectCurriculumRepository, IUserRepository userRepository)
        {
            _chapterRepository = chapterRepository;
            _subjectCurriculumRepository = subjectCurriculumRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.Id) ?? throw new ChapterNotFoundException();


            if (await _subjectCurriculumRepository.GetByIdAsync(request.SubjectCurriculumId) == null)
                throw new SubjectCurriculumNotFoundException();

            if (!Guid.TryParse(request.UpdatedBy, out Guid updatedByGuid))
                throw new InvalidOperationException("Invalid GUID format for UpdatedBy.");

            if (await _userRepository.GetByIdAsync(updatedByGuid) == null)
                throw new UserNotFoundException();

            chapter.Name = request.Name;
            chapter.SubjectCurriculumId = request.SubjectCurriculumId;
            chapter.OrderNo = request.OrderNo;
            chapter.UpdatedBy = request.UpdatedBy;
            chapter.UpdatedAt = DateTime.UtcNow;

            await _chapterRepository.UpdateAsync(chapter);
            return Unit.Value;
        }
    }
}
