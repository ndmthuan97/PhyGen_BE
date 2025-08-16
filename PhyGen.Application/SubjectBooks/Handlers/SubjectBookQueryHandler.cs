using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Exceptions;
using PhyGen.Application.SubjectBooks.Queries;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
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

    public class GetSubjectBooksBySubjectIdQueryHandler : IRequestHandler<GetSubjectBooksBySubjectIdQuery, Pagination<SubjectBookResponse>>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        private readonly ISubjectRepository _subjectRepository;

        public GetSubjectBooksBySubjectIdQueryHandler(ISubjectBookRepository subjectBookRepository, ISubjectRepository subjectRepository)
        {
            _subjectBookRepository = subjectBookRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<Pagination<SubjectBookResponse>> Handle(GetSubjectBooksBySubjectIdQuery request, CancellationToken cancellationToken)
        {
            var subjectBooks = await _subjectBookRepository.GetSubjectBooksBySubjectWithSpecAsync(request.SubjectBookSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<SubjectBookResponse>>(subjectBooks);
        }
    }

    public class GetSubjectBookByGradeQueryHandler : IRequestHandler<GetSubjectBookByGrade, List<SubjectBookResponse>>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        public GetSubjectBookByGradeQueryHandler(ISubjectBookRepository subjectBookRepository)
        {
            _subjectBookRepository = subjectBookRepository;
        }
        public async Task<List<SubjectBookResponse>> Handle(GetSubjectBookByGrade request, CancellationToken cancellationToken)
        {
            var subjectBooks = await _subjectBookRepository.GetSubjectBooksByGradeAsync(request.Grade);
            subjectBooks = subjectBooks?.Where(sb => !sb.DeletedAt.HasValue).ToList();
            if (subjectBooks == null || !subjectBooks.Any())
            {
                throw new SubjectBookNotFoundException();
            }
            return AppMapper<CoreMappingProfile>.Mapper.Map<List<SubjectBookResponse>>(subjectBooks);
        }
    }

    public class GetNamesByTopicIdQueryHandler : IRequestHandler<GetNamesByTopicIdQuery, object>
    {
        private readonly ISubjectBookRepository _subjectBookRepository;
        public GetNamesByTopicIdQueryHandler(ISubjectBookRepository subjectBookRepository)
        {
            _subjectBookRepository = subjectBookRepository;
        }
        public async Task<object> Handle(GetNamesByTopicIdQuery request, CancellationToken cancellationToken)
        {
            var names = await _subjectBookRepository.GetNamesByTopicIdAsync(request.TopicId);
            if (names == null)
            {
                throw new SubjectNotFoundException();
            }
            return names;
        }
    }
}
