using MediatR;
using PhyGen.Application.Answers.Queries;
using PhyGen.Application.Answers.Responses;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Handlers
{
    public class GetAllAnswersQueryHandler : IRequestHandler<GetAllAnswersQuery, List<AnswerResponse>>
    {
        private readonly IAnswerRepository _answerRepository;

        public GetAllAnswersQueryHandler(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<List<AnswerResponse>> Handle(GetAllAnswersQuery request, CancellationToken cancellationToken)
        {
            var answers = await _answerRepository.GetAllAsync();

            var answerResponses = AppMapper<CoreMappingProfile>.Mapper.Map<List<AnswerResponse>>(answers);

            return answerResponses;
        }
    }
}
