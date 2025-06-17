using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Exceptions;
using PhyGen.Application.SubjectBooks.Queries;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Handlers
{
    public class GetSubjectBookByIdQueryHandler : IRequestHandler<GetSubjectBookByIdQuery, SubjectBookResponse>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;

        public GetSubjectBookByIdQueryHandler(ISubjectBookRepository subjectBookRepository)
        {
            _subjectBookRepository = subjectBookRepository;
        }

        public async Task<SubjectBookResponse> Handle(GetSubjectBookByIdQuery request, CancellationToken cancellationToken)
        {
            var subjectBook = await _subjectBookRepository.GetByIdAsync(request.Id) ?? throw new SubjectBookNotFoundException();
            
            return AppMapper<CoreMappingProfile>.Mapper.Map<SubjectBookResponse>(subjectBook);
        }
    }

    public class GetSubjectBooksBySubjectIdQueryHandler : IRequestHandler<GetSubjectBooksBySubjectIdQuery, List<SubjectBookResponse>>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        private readonly ISubjectRepository _subjectRepository;

        public GetSubjectBooksBySubjectIdQueryHandler(ISubjectBookRepository subjectBookRepository, ISubjectRepository subjectRepository)
        {
            _subjectBookRepository = subjectBookRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<List<SubjectBookResponse>> Handle(GetSubjectBooksBySubjectIdQuery request, CancellationToken cancellationToken)
        {
            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            var subjectBooks = await _subjectBookRepository.GetSubjectBooksBySubjectIdAsync(request.SubjectId);

            if (subjectBooks == null || !subjectBooks.Any())
                throw new SubjectBookNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<SubjectBookResponse>>(subjectBooks.OrderBy(sb => sb.Name));
        }
    }
}
