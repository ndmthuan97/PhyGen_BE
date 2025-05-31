using MediatR;
using PhyGen.Application.ChapterUnits.Commands;
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
    public class DeleteChapterUnitCommandHandler : IRequestHandler<DeleteChapterUnitCommand, Unit>
    {
        private readonly IChapterUnitRepository _chapterUnitRepository;
        private readonly IUserRepository _userRepository;

        public DeleteChapterUnitCommandHandler(IChapterUnitRepository chapterUnitRepository, IUserRepository userRepository)
        {
            _chapterUnitRepository = chapterUnitRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteChapterUnitCommand request, CancellationToken cancellationToken)
        {
            var chapterUnit = await _chapterUnitRepository.GetByIdAsync(request.ChapterUnitId) ?? throw new ChapterUnitNotFoundException();
            
            if (await _userRepository.GetUserByEmailAsync(request.DeletedBy) == null)
                throw new UserNotFoundException();

            chapterUnit.DeletedBy = request.DeletedBy;
            chapterUnit.DeletedAt = DateTime.UtcNow;

            await _chapterUnitRepository.UpdateAsync(chapterUnit);
            return Unit.Value;
        }
    }
}
