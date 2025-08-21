using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.QuestionSections.Exceptions;
using PhyGen.Application.QuestionSections.Queries;
using PhyGen.Application.QuestionSections.Responses;
using PhyGen.Application.Sections.Exceptions;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Handlers
{
    public class GetQuestionSectionByIdQueryHandler : IRequestHandler<GetQuestionSectionByIdQuery, QuestionSectionResponse>
    {
        private readonly IQuestionSectionRepository _questionSectionRepository;

        public GetQuestionSectionByIdQueryHandler(IQuestionSectionRepository questionSectionRepository)
        {
            _questionSectionRepository = questionSectionRepository;
        }

        public async Task<QuestionSectionResponse> Handle(GetQuestionSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var questionSection = await _questionSectionRepository.GetByIdAsync(request.Id);

            if (questionSection == null)
            {
                throw new QuestionSectionNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionSectionResponse>(questionSection);
        }
    }

    public class GetQuestionSectionsByQuestionIdAndSectionIdQueryHandler : IRequestHandler<GetQuestionSectionsByQuestionIdAndSectionIdQuery, QuestionSectionResponse>
    {
        private readonly IQuestionSectionRepository _questionSectionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISectionRepository _sectionRepository;

        public GetQuestionSectionsByQuestionIdAndSectionIdQueryHandler(
            IQuestionSectionRepository questionSectionRepository,
            IQuestionRepository questionRepository,
            ISectionRepository sectionRepository)
        {
            _questionSectionRepository = questionSectionRepository;
            _questionRepository = questionRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<QuestionSectionResponse> Handle(GetQuestionSectionsByQuestionIdAndSectionIdQuery request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            if (await _sectionRepository.GetByIdAsync(request.SectionId) == null)
            {
                throw new SectionNotFoundException();
            }

            var questionSections = await _questionSectionRepository.GetQuestionSectionByQuestionIdAndSectionIdAsync(request.QuestionId, request.SectionId);

            if (questionSections == null)
            {
                throw new QuestionSectionNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionSectionResponse>(questionSections);
        }
    }
}
