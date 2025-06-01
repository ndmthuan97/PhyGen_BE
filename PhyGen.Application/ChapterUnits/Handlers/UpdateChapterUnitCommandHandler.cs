using MediatR;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.ChapterUnits;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Handlers
{
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
            var chapterUnit = await _chapterUnitRepository.GetByIdAsync(request.ChapterUnitId) ?? throw new ChapterUnitNotFoundException();
            
            if (await _userRepository.GetUserByEmailAsync(request.UpdatedBy) == null)
                throw new UserNotFoundException();
            
            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            chapterUnit.Name = request.Name;
            chapterUnit.ChapterId = request.ChapterId;
            chapterUnit.OrderNo = request.OrderNo;
            chapterUnit.UpdatedBy = request.UpdatedBy;
            chapterUnit.UpdatedAt = DateTime.UtcNow;

            await _chapterUnitRepository.UpdateAsync(chapterUnit);
            return Unit.Value;
        }
    }
}
