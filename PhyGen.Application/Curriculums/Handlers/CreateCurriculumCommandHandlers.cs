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
    public class CreateCurriculumCommandHandlers : IRequestHandler<CreateCurriculumCommand, Guid>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public CreateCurriculumCommandHandlers(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Guid> Handle(CreateCurriculumCommand request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetCurriculumByNameAsync(request.Name) != null) 
                throw new CurriculumSameNameException();

            var curriculum = new Curriculum
            {
                Name = request.Name,
                Grade = request.Grade,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            curriculum = await _curriculumRepository.AddAsync(curriculum);
            return curriculum.Id;
        }
    }
}
