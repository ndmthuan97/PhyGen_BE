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
            var examResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<ExamResponse>>(exams);
            return examResponses;
        }
    }
}
