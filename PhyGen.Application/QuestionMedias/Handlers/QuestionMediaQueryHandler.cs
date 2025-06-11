using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.QuestionMedias.Exceptions;
using PhyGen.Application.QuestionMedias.Queries;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Handlers
{
    public class GetQuestionMediaByIdQueryHandler : IRequestHandler<GetQuestionMediaByIdQuery, QuestionMediaResponse>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        
        public GetQuestionMediaByIdQueryHandler(IQuestionMediaRepository questionMediaRepository)
        {
            _questionMediaRepository = questionMediaRepository;
        }

        public async Task<QuestionMediaResponse> Handle(GetQuestionMediaByIdQuery request, CancellationToken cancellationToken)
        {
            var questionMedia = await _questionMediaRepository.GetByIdAsync(request.QuestionMediaId)
                ?? throw new QuestionMediaNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionMediaResponse>(questionMedia);
        }
    }
}
