using MediatR;
using PhyGen.Application.Answers.Queries;
using PhyGen.Application.Answers.Responses;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Answers.Handlers
{
    public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, AnswerResponse>
    {
        private readonly IAnswerRepository _answerRepository;

        public GetAnswerByIdQueryHandler(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<AnswerResponse> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
        {
            var answer = await _answerRepository.GetByIdAsync(request.AnswerId) ?? throw new CultureNotFoundException();
            return AppMapper<CoreMappingProfile>.Mapper.Map<AnswerResponse>(answer);
        }
    }
}
