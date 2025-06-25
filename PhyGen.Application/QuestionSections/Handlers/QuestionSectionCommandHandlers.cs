using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.QuestionSections.Commands;
using PhyGen.Application.QuestionSections.Exceptions;
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
    public class CreateQuestionSectionCommandHandler : IRequestHandler<CreateQuestionSectionCommand, QuestionSectionResponse>
    {
        private readonly IQuestionSectionRepository _questionSectionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISectionRepository _sectionRepository;

        public CreateQuestionSectionCommandHandler(
            IQuestionSectionRepository questionSectionRepository,
            IQuestionRepository questionRepository,
            ISectionRepository sectionRepository)
        {
            _questionSectionRepository = questionSectionRepository;
            _questionRepository = questionRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<QuestionSectionResponse> Handle(CreateQuestionSectionCommand request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            if (await _sectionRepository.GetByIdAsync(request.SectionId) == null)
            {
                throw new SectionNotFoundException();
            }

            var isExist = await _questionSectionRepository.GetQuestionSectionByQuestionIdAndSectionIdAsync(request.QuestionId, request.SectionId);
            if (isExist != null)
            {
                throw new QuestionSectionAlreadyExistException();
            }

            var questionSection = new Domain.Entities.QuestionSection
            {
                QuestionId = request.QuestionId,
                SectionId = request.SectionId,
                Score = request.Score
            };

            await _questionSectionRepository.AddAsync(questionSection);
            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionSectionResponse>(questionSection);
        }
    }

    public class UpdateQuestionSectionCommandHandler : IRequestHandler<UpdateQuestionSectionCommand, Unit>
    {
        private readonly IQuestionSectionRepository _questionSectionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISectionRepository _sectionRepository;
        public UpdateQuestionSectionCommandHandler(
            IQuestionSectionRepository questionSectionRepository,
            IQuestionRepository questionRepository,
            ISectionRepository sectionRepository)
        {
            _questionSectionRepository = questionSectionRepository;
            _questionRepository = questionRepository;
            _sectionRepository = sectionRepository;
        }
        public async Task<Unit> Handle(UpdateQuestionSectionCommand request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            if (await _sectionRepository.GetByIdAsync(request.SectionId) == null)
            {
                throw new SectionNotFoundException();
            }

            var questionSection = await _questionSectionRepository.GetQuestionSectionByQuestionIdAndSectionIdAsync(request.QuestionId, request.SectionId);
            if (questionSection == null)
            {
                throw new QuestionSectionNotFoundException();
            }

            questionSection.Score = request.Score;

            await _questionSectionRepository.UpdateAsync(questionSection);
            return Unit.Value;
        }
    }
}
