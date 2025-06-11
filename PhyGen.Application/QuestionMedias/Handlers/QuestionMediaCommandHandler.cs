using MediatR;
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
    public class CreateQuestionMediaCommandHandler : IRequestHandler<CreateQuestionMediaCommand, Guid>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        private readonly IQuestionRepository _questionRepository;

        public CreateQuestionMediaCommandHandler(IQuestionMediaRepository questionMediaRepository, IQuestionRepository questionRepository)
        {
            _questionMediaRepository = questionMediaRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Guid> Handle(CreateQuestionMediaCommand request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
                throw new QuestionNotFoundException();

            var questionMedia = new QuestionMedia
            {
                QuestionId = request.QuestionId,
                MediaType = request.MediaType,
                Url = request.Url
            };

            await _questionMediaRepository.AddAsync(questionMedia);
            return questionMedia.Id;
        }
    }

    public class UpdateQuestionMediaCommandHandler : IRequestHandler<UpdateQuestionMediaCommand, Unit>
    {
        private readonly IQuestionMediaRepository _questionMediaRepository;
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionMediaCommandHandler(IQuestionMediaRepository questionMediaRepository, IQuestionRepository questionRepository)
        {
            _questionMediaRepository = questionMediaRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionMediaCommand request, CancellationToken cancellationToken)
        {
            if (await _questionMediaRepository.GetByIdAsync(request.QuestionMediaId) == null)
                throw new QuestionMediaNotFoundException();

            if (await _questionRepository.GetByIdAsync(request.QuestionId) == null)
                throw new QuestionNotFoundException();

            var questionMedia = new QuestionMedia
            {
                Id = request.QuestionMediaId,
                QuestionId = request.QuestionId,
                MediaType = request.MediaType,
                Url = request.Url,
            };
            
            await _questionMediaRepository.UpdateAsync(questionMedia);
            return Unit.Value;
        }
    }
}
