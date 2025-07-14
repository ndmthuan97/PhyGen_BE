using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.Topics.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Handlers
{
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;

        public CreateQuestionCommandHandler(IQuestionRepository questionRepository, ITopicRepository topicRepository)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
        }

        public async Task<QuestionResponse> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null || topic.DeletedAt.HasValue)
                throw new TopicNotFoundException();

            var isExist = await _questionRepository.AlreadyExistAsync(q =>
                q.TopicId == request.TopicId &&
                q.Content.ToLower() == request.Content.ToLower() &&
                q.Level == request.Level &&
                q.Type == request.Type &&
                q.DeletedAt == null
            );
            if (isExist)
                throw new QuestionAlreadyExistException();

            var question = new Question
            {
                TopicId = request.TopicId,
                Content = request.Content,
                Level = request.Level,
                Type = request.Type,
                Image = request.Image,
                Answer1 = request.Answer1,
                Answer2 = request.Answer2,
                Answer3 = request.Answer3,
                Answer4 = request.Answer4,
                Answer5 = request.Answer5,
                Answer6 = request.Answer6,
                CorrectAnswer = request.CorrectAnswer,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };

            await _questionRepository.AddAsync(question);
            return AppMapper<CoreMappingProfile>.Mapper.Map<QuestionResponse>(question);
        }
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, ITopicRepository topicRepository)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question == null || question.DeletedAt.HasValue)
                throw new QuestionNotFoundException();

            var topic = await _topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null || topic.DeletedAt.HasValue)
                throw new TopicNotFoundException();

            var isExist = await _questionRepository.AlreadyExistAsync(q =>
                q.Id != request.Id &&
                q.TopicId == request.TopicId &&
                q.Content.ToLower() == request.Content.ToLower() &&
                q.Level == request.Level &&
                q.Type == request.Type &&
                q.DeletedAt == null
            );
            if (isExist)
                throw new QuestionAlreadyExistException();

            question.TopicId = request.TopicId;
            question.Content = request.Content;
            question.Level = request.Level;
            question.Type = request.Type;
            question.Image = request.Image;
            question.Answer1 = request.Answer1;
            question.Answer2 = request.Answer2;
            question.Answer3 = request.Answer3;
            question.Answer4 = request.Answer4;
            question.Answer5 = request.Answer5;
            question.Answer6 = request.Answer6;
            question.CorrectAnswer = request.CorrectAnswer;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question == null || question.DeletedAt.HasValue)
                throw new QuestionNotFoundException();

            question.DeletedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }
}
