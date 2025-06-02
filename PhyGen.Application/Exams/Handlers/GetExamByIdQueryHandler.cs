using MediatR;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
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
            var exam = await _examRepository.GetByIdAsync(request.ExamId) ?? throw new KeyNotFoundException("Exam not found.");
            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamResponse>(exam);
        }
    }
}
