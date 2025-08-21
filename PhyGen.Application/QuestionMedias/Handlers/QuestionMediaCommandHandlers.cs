using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.QuestionMedias.Commands;
using PhyGen.Application.QuestionMedias.Exceptions;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionMedias.Handlers
{
    public class CreateQuestionMediaCommandHandler : IRequestHandler<CreateQuestionMediaCommand, QuestionMediaResponse>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        private readonly IQuestionRepository _questionRepository;

        public CreateQuestionMediaCommandHandler(
            IQuestionMediaRepository questionMediaRepository,
            IQuestionRepository questionRepository)
        {
            _questionMediaRepository = questionMediaRepository;
            _questionRepository = questionRepository;
        }

        public async Task<QuestionMediaResponse> Handle(CreateQuestionMediaCommand request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            var questionMedia = new QuestionMedia
            {
                QuestionId = request.QuestionId,
                MediaType = request.MediaType,
                Url = request.Url
            };

            await _questionMediaRepository.AddAsync(questionMedia);
            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionMediaResponse>(questionMedia);
        }
    }

    public class UpdateQuestionMediaCommandHandler : IRequestHandler<UpdateQuestionMediaCommand, Unit>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionMediaCommandHandler(
            IQuestionMediaRepository questionMediaRepository,
            IQuestionRepository questionRepository)
        {
            _questionMediaRepository = questionMediaRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionMediaCommand request, CancellationToken cancellationToken)
        {
            var questionMedia = await _questionMediaRepository.GetByIdAsync(request.Id);

            if (questionMedia == null)
            {
                throw new QuestionMediaNotFoundException();
            }

            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
            {
                throw new QuestionNotFoundException();
            }

            questionMedia.QuestionId = request.QuestionId;
            questionMedia.MediaType = request.MediaType;
            questionMedia.Url = request.Url;

            await _questionMediaRepository.UpdateAsync(questionMedia);
            return Unit.Value;
        }
    }

    public class DeleteQuestionMediaCommandHandler : IRequestHandler<DeleteQuestionMediaCommand, Unit>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;

        public DeleteQuestionMediaCommandHandler(IQuestionMediaRepository questionMediaRepository)
        {
            _questionMediaRepository = questionMediaRepository;
        }

        public async Task<Unit> Handle(DeleteQuestionMediaCommand request, CancellationToken cancellationToken)
        {
            var questionMedia = await _questionMediaRepository.GetByIdAsync(request.Id);

            if (questionMedia == null)
            {
                throw new QuestionMediaNotFoundException();
            }

            await _questionMediaRepository.DeleteAsync(questionMedia);
            return Unit.Value;
        }
    }
}
