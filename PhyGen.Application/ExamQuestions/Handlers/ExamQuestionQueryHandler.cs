using MediatR;
using PhyGen.Application.ExamQuestions.Exceptions;
using PhyGen.Application.ExamQuestions.Queries;
using PhyGen.Application.ExamQuestions.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Handlers
{
    public class GetAllExamQuestionsQueryHandler : IRequestHandler<GetAllExamQuestionsQuery, List<ExamQuestionResponse>>
    {
        private readonly IExamQuestionRepository _examQuestionRepository;

        public GetAllExamQuestionsQueryHandler(IExamQuestionRepository examQuestionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
        }

        public async Task<List<ExamQuestionResponse>> Handle(GetAllExamQuestionsQuery request, CancellationToken cancellationToken)
        {
            var examQuestions = await _examQuestionRepository.GetAllAsync();
            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ExamQuestionResponse>>(examQuestions);
        }
    }
    
    public class GetExamQuestionByIdQueryHandler : IRequestHandler<GetExamQuestionByIdQuery, ExamQuestionResponse>
    {
        private readonly IExamQuestionRepository _examQuestionRepository;

        public GetExamQuestionByIdQueryHandler(IExamQuestionRepository examQuestionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
        }

        public async Task<ExamQuestionResponse> Handle(GetExamQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var examQuestion = await _examQuestionRepository.GetByIdAsync(request.ExamQuestionId)
                ?? throw new ExamQuestionNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamQuestionResponse>(examQuestion);
        }
    }
}
