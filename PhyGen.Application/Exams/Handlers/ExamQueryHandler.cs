using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, List<ExamResponse>>
    {
        private readonly IExamRepository _examRepository;

        public GetAllExamsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<List<ExamResponse>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
        {
            var exams = await _examRepository.GetAllAsync();
            var activeExams = exams.Where(e => e.DeletedBy == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ExamResponse>>(activeExams);
        }
    }

    public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamResponse>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamResponse> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.ExamId) 
                ?? throw new ExamNotFoundException();

            if (exam.DeletedBy != null)
                throw new ExamNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamResponse>(exam);
        }
    }
}
