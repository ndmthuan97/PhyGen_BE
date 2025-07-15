using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Responses;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class GetQuestionByIdQueryHandlers : IRequestHandler<GetQuestionByIdQuery, QuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionByIdQueryHandlers(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionResponse> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId);
            if (question == null || question.DeletedAt.HasValue)
                throw new QuestionNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionResponse>(question);
        }
    }

    public class GetQuestionsByTopicIdQueryHandler : IRequestHandler<GetQuestionsByTopicIdQuery, Pagination<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionsByTopicIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Pagination<QuestionResponse>> Handle(GetQuestionsByTopicIdQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsByTopicAsync(request.param);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<QuestionResponse>>(questions);
        }
    }

    public class GetQuestionsByLevelAndTypeQueryHandler : IRequestHandler<GetQuestionsByLevelAndTypeQuery, Pagination<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionsByLevelAndTypeQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Pagination<QuestionResponse>> Handle(GetQuestionsByLevelAndTypeQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsByLevelAndTopicAsync(request.QuestionSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<QuestionResponse>>(questions);
        }
    }

    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, Pagination<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionsQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Pagination<QuestionResponse>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsAsync(request.QuestionSpecParam);
            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<QuestionResponse>>(questions);
        }
    }

    public class GetQuestionsByGradeQueryHandler : IRequestHandler<GetQuestionsByGradeQuery, Pagination<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionsByGradeQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Pagination<QuestionResponse>> Handle(GetQuestionsByGradeQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsByGradeAsync(request.questionGradeParam);
            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<QuestionResponse>>(questions);
        }
    }
}
