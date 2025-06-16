using MediatR;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.SubjectCurriculums.Commands;
using PhyGen.Application.SubjectCurriculums.Exceptions;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Handlers
{
    public class CreateSubjectCurriculumCommandHandler : IRequestHandler<CreateSubjectCurriculumCommand, Guid>
    {
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICurriculumRepository _curriculumRepository;

        public CreateSubjectCurriculumCommandHandler(
            ISubjectCurriculumRepository subjectCurriculumRepository,
            ISubjectRepository subjectRepository,
            ICurriculumRepository curriculumRepository)
        {
            _subjectCurriculumRepository = subjectCurriculumRepository;
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Guid> Handle(CreateSubjectCurriculumCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            var subjectCurriculum = new SubjectCurriculum
            {
                SubjectId = request.SubjectId,
                CurriculumId = request.CurriculumId
            };

            await _subjectCurriculumRepository.AddAsync(subjectCurriculum);
            return subjectCurriculum.SubjectId;
        }
    }

    public class UpdateSubjectCurriculumCommandHandler : IRequestHandler<UpdateSubjectCurriculumCommand, Unit>
    {
        private readonly ISubjectCurriculumRepository _subjectCurriculumRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICurriculumRepository _curriculumRepository;

        public UpdateSubjectCurriculumCommandHandler(
            ISubjectCurriculumRepository subjectCurriculumRepository,
            ISubjectRepository subjectRepository,
            ICurriculumRepository curriculumRepository)
        {
            _subjectCurriculumRepository = subjectCurriculumRepository;
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<Unit> Handle(UpdateSubjectCurriculumCommand request, CancellationToken cancellationToken)
        {
            var subjectCurriculum = await _subjectCurriculumRepository.GetByIdAsync(request.Id) ?? throw new SubjectCurriculumNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            if (await _curriculumRepository.GetByIdAsync(request.CurriculumId) == null)
                throw new CurriculumNotFoundException();

            subjectCurriculum.SubjectId = request.SubjectId;
            subjectCurriculum.CurriculumId = request.CurriculumId;

            await _subjectCurriculumRepository.UpdateAsync(subjectCurriculum);
            return Unit.Value;
        }
    }
}
