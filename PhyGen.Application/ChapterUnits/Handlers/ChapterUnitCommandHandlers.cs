using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ChapterUnits.Exceptions;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Handlers
{
    public class CreateChapterUnitCommandHandler : IRequestHandler<CreateChapterUnitCommand, Guid>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;

        public CreateChapterUnitCommandHandler(IChapterUnitRepository chapterUnitRepository, IChapterRepository chapterRepository, IUserRepository userRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateChapterUnitCommand request, CancellationToken cancellationToken)
        {
            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            if (!Guid.TryParse(request.CreatedBy, out Guid createdByGuid))
                throw new InvalidOperationException("Invalid GUID format for CreatedBy.");

            if (await _userRepository.GetByIdAsync(createdByGuid) == null)
                throw new UserNotFoundException();

            var chapterUnit = new ChapterUnit
            {
                Name = request.Name,
                Description = request.Description,
                ChapterId = request.ChapterId,
                OrderNo = request.OrderNo,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _chapterUnitRepository.AddAsync(chapterUnit);
            return chapterUnit.Id;
        }
    }

    public class UpdateChapterUnitCommandHandler : IRequestHandler<UpdateChapterUnitCommand, Unit>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChapterRepository _chapterRepository;

        public UpdateChapterUnitCommandHandler(IChapterUnitRepository chapterUnitRepository, IUserRepository userRepository, IChapterRepository chapterRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
            _userRepository = userRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Unit> Handle(UpdateChapterUnitCommand request, CancellationToken cancellationToken)
        {
            var chapterUnit = await _chapterUnitRepository.GetByIdAsync(request.Id) ?? throw new ChapterUnitNotFoundException();

            if (!Guid.TryParse(request.UpdatedBy, out Guid updatedByGuid))
                throw new InvalidOperationException("Invalid GUID format for UpdatedBy.");

            if (await _userRepository.GetByIdAsync(updatedByGuid) == null)
                throw new UserNotFoundException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            chapterUnit.Name = request.Name;
            chapterUnit.Description = request.Description;
            chapterUnit.ChapterId = request.ChapterId;
            chapterUnit.OrderNo = request.OrderNo;
            chapterUnit.UpdatedBy = request.UpdatedBy;
            chapterUnit.UpdatedAt = DateTime.UtcNow;

            await _chapterUnitRepository.UpdateAsync(chapterUnit);
            return Unit.Value;
        }
    }
}
