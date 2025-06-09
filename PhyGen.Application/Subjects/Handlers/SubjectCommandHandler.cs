using MediatR;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Application.Subjects.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Subjects.Handlers
{
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Guid>
    {
        private readonly ISubjectRepository _subjectRepository;

        public CreateSubjectCommandHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<Guid> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectRepository.GetSubjectByNameAsync(request.Name) != null)
                throw new SubjectSameNameException();

            var subject = new Subject
            {
                Name = request.Name
            };

            await _subjectRepository.AddAsync(subject);
            return subject.Id;
        }
    }

    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Unit>
    {
        private readonly ISubjectRepository _subjectRepository;

        public UpdateSubjectCommandHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<Unit> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _subjectRepository.GetByIdAsync(request.SubjectId) ?? throw new SubjectNotFoundException();

            if (await _subjectRepository.GetSubjectByNameAsync(request.Name) != null)
                throw new SubjectSameNameException();

            subject.Name = request.Name;

            await _subjectRepository.UpdateAsync(subject);
            return Unit.Value;
        }
    }
}
