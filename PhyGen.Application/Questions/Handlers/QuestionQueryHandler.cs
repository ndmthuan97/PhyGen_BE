using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, List<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetAllQuestionsQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<List<QuestionResponse>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync();
            var activeQuestions = questions.Where(q => q.DeletedBy == null).ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<QuestionResponse>>(activeQuestions);
        }
    }

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
                ?? throw new QuestionNotFoundException();

            if (question.DeletedBy != null)
                throw new QuestionNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionResponse>(question);
        }
    }
}
