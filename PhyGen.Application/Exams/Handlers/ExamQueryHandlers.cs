using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamResponse>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamResponse> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.Id)
                ?? throw new ExamNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamResponse>(exam);
        }
    }

    public class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, Pagination<ExamResponse>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Pagination<ExamResponse>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
        {
            var exams = await _examRepository.GetExamsAsync(request.ExamSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<ExamResponse>>(exams);
        }
    }
}
