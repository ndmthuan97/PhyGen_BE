using MediatR;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Exceptions.Curriculums;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class UpdateCurriculumCommandHandler : IRequestHandler<UpdateCurriculumCommand, Unit>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public UpdateCurriculumCommandHandler(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Unit> Handle(UpdateCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId);

            if (curriculum == null)
                throw new CurriculumNotFoundException();

            if (await _curriculumRepository.GetCurriculumByNameAsync(request.Name) != null)
                throw new CurriculumSameNameException();

            curriculum.Name = request.Name;
            curriculum.Grade = request.Grade;
            curriculum.Description = request.Description;
            curriculum.UpdatedBy = request.UpdatedBy;
            curriculum.UpdatedAt = DateTime.UtcNow;

            await _curriculumRepository.UpdateAsync(curriculum);

            return Unit.Value;
        }
    }
}
