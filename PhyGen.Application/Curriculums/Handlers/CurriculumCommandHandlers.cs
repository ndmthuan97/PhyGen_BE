using MediatR;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Curriculums.Response;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Curriculums.Handlers
{
    public class CreateCurriculumCommandHandlers : IRequestHandler<CreateCurriculumCommand, CurriculumResponse>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public CreateCurriculumCommandHandlers(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<CurriculumResponse> Handle(CreateCurriculumCommand request, CancellationToken cancellationToken)
        {
            if (await _curriculumRepository.GetCurriculumByNameAsync(request.Name) != null)
                throw new CurriculumSameNameException();

            var curriculum = new Curriculum
            {
                Name = request.Name,
                Grade = request.Grade
            };

            curriculum = await _curriculumRepository.AddAsync(curriculum);
            return AppMapper<CoreMappingProfile>.Mapper.Map<CurriculumResponse>(curriculum);
        }
    }

    public class UpdateCurriculumCommandHandler : IRequestHandler<UpdateCurriculumCommand, Unit>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public UpdateCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUserRepository userRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Unit> Handle(UpdateCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.Id);

            if (curriculum == null)
                throw new CurriculumNotFoundException();

            if (await _curriculumRepository.GetCurriculumByNameAsync(request.Name) != null)
                throw new CurriculumSameNameException();

            curriculum.Name = request.Name;
            curriculum.Grade = request.Grade;

            await _curriculumRepository.UpdateAsync(curriculum);

            return Unit.Value;
        }
    }
}
