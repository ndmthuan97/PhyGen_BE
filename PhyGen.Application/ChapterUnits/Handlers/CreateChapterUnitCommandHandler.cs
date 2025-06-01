using MediatR;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.Exceptions.Chapters;
using PhyGen.Application.Exceptions.ChapterUnits;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
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
            if (await _chapterUnitRepository.GetChapterUnitByNameAsync(request.Name) != null)
                throw new ChapterUnitSameNameException();

            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            if (await _userRepository.GetUserByEmailAsync(request.CreatedBy) == null)
                throw new UserNotFoundException();

            var chapterUnit = new ChapterUnit
            {
                Name = request.Name,
                ChapterId = request.ChapterId,
                OrderNo = request.OrderNo,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _chapterUnitRepository.AddAsync(chapterUnit);
            return chapterUnit.Id;
        }
    }
}
