using MediatR;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Application.Exceptions.Users;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class DeleteCurriculumCommandHandler : IRequestHandler<DeleteCurriculumCommand, Unit>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUserRepository _userRepository;

        public DeleteCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUserRepository userRepository)
        {
            _curriculumRepository = curriculumRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId) ?? throw new CurriculumNotFoundException();

            if (await _userRepository.GetUserByEmailAsync(request.DeletedBy) == null)
                throw new UserNotFoundException();

            curriculum.DeletedBy = request.DeletedBy;
            curriculum.DeletedAt = DateTime.UtcNow;

            await _curriculumRepository.UpdateAsync(curriculum);

            return Unit.Value;
        }
    }
}
