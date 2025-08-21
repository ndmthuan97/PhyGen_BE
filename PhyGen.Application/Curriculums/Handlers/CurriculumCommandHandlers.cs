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
            if (await _curriculumRepository.AlreadyExistAsync(x =>
                x.Name.ToLower() == request.Name.ToLower() &&
                x.Year == request.Year &&
                x.DeletedAt == null
                ))
                throw new CurriculumAlreadyExistException();

            if (request.Year < 2018 || request.Year > DateTime.Now.Year)
                throw new Exception("Năm phải nằm trong khoảng từ 2018 đến hiện tại.");

            var curriculum = new Curriculum
            {
                Name = request.Name,
                Year = request.Year,
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

            if (curriculum == null || curriculum.DeletedAt.HasValue)
                throw new CurriculumNotFoundException();

            if (await _curriculumRepository.AlreadyExistAsync(x =>
                x.Name.ToLower() == request.Name.ToLower() &&
                x.Year == request.Year &&
                x.DeletedAt == null
                ))
                throw new CurriculumAlreadyExistException();

            if (request.Year < 2018 || request.Year > DateTime.Now.Year)
                throw new Exception("Năm phải nằm trong khoảng từ 2018 đến hiện tại.");

            curriculum.Name = request.Name;
            curriculum.Year = request.Year;

            await _curriculumRepository.UpdateAsync(curriculum);

            return Unit.Value;
        }
    }

    public class DeleteCurriculumCommandHandler : IRequestHandler<DeleteCurriculumCommand, Unit>
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public DeleteCurriculumCommandHandler(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Unit> Handle(DeleteCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.Id) ?? throw new CurriculumNotFoundException();

            curriculum.DeletedAt = DateTime.UtcNow;

            await _curriculumRepository.UpdateAsync(curriculum);
            return Unit.Value;
        }
    }
}
