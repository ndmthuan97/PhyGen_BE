using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.QuestionMedias.Exceptions;
using PhyGen.Application.QuestionMedias.Queries;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Application.Questions.Exceptions;
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
            var questionMedia = await _questionMediaRepository.GetByIdAsync(request.Id);

            if (questionMedia == null)
            {
                throw new QuestionMediaNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionMediaResponse>(questionMedia);
        }
    }

    public class GetQuestionMediasByQuestionIdQueryHandler : IRequestHandler<GetQuestionMediasByQuestionIdQuery, IEnumerable<QuestionMediaResponse>>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionMediasByQuestionIdQueryHandler(IQuestionMediaRepository questionMediaRepository, IQuestionRepository questionRepository)
        {
            _questionMediaRepository = questionMediaRepository;
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionMediaResponse>> Handle(GetQuestionMediasByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            var questionMedias = await _questionMediaRepository.GetQuestionMediaByQuestionIdAsync(request.QuestionId);

            if (questionMedias == null)
            {
                throw new QuestionMediaNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<QuestionMediaResponse>>(questionMedias);
        }
    }
}
