using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Commands;
using PhyGen.Application.SubjectBooks.Exceptions;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Handlers
{
    public class CreateSubjectBookCommandHandler : IRequestHandler<CreateSubjectBookCommand, SubjectBookResponse>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        private readonly ISubjectRepository _subjectRepository;

        public CreateSubjectBookCommandHandler(ISubjectBookRepository subjectBookRepository, ISubjectRepository subjectRepository)
        {
            _subjectBookRepository = subjectBookRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<SubjectBookResponse> Handle(CreateSubjectBookCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectBookNotFoundException();

            if (await _subjectBookRepository.GetSubjectBooksBySubjectIdAsync(request.SubjectId) == null)
                throw new SubjectBookSameNameException();

            var subjectBook = new SubjectBook
            {
                SubjectId = request.SubjectId,
                Name = request.Name,
                Grade = request.Grade
            };

            await _subjectBookRepository.AddAsync(subjectBook);
            return AppMapper<CoreMappingProfile>.Mapper.Map<SubjectBookResponse>(subjectBook);
        }
    }

    public class UpdateSubjectBookCommandHandler : IRequestHandler<UpdateSubjectBookCommand, Unit>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        private readonly ISubjectRepository _subjectRepository;

        public UpdateSubjectBookCommandHandler(ISubjectBookRepository subjectBookRepository, ISubjectRepository subjectRepository)
        {
            _subjectBookRepository = subjectBookRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<Unit> Handle(UpdateSubjectBookCommand request, CancellationToken cancellationToken)
        {
            var subjectBook = await _subjectBookRepository.GetByIdAsync(request.Id) ?? throw new SubjectBookNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectBookNotFoundException();

            if (await _subjectBookRepository.GetSubjectBooksBySubjectIdAsync(request.SubjectId) == null)
                throw new SubjectBookSameNameException();

            subjectBook.SubjectId = request.SubjectId;
            subjectBook.Name = request.Name;
            subjectBook.Grade = request.Grade;

            await _subjectBookRepository.UpdateAsync(subjectBook);
            return Unit.Value;
        }
    }
}
