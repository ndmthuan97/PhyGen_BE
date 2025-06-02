using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionResponse> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId) 
                ?? throw new KeyNotFoundException("Question not found.");
            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionResponse>(question);
        }
    }
}
