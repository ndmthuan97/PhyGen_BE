using MediatR;
using Microsoft.AspNetCore.Http;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Exceptions;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.Status;
using PhyGen.Application.Topics.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (request.TopicId.HasValue)
            {
                var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);
                if (topic == null || topic.DeletedAt.HasValue)
                    throw new TopicNotFoundException();
            }

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
                Answer1 = request.Answer1,
                Answer2 = request.Answer2,
                Answer3 = request.Answer3,
                Answer4 = request.Answer4,
                Answer5 = request.Answer5,
                Answer6 = request.Answer6,
                Grade = request.Grade,
                Status = request.Status,
                QuestionCode = await _questionRepository.GenerateCodeAsync<Question>("Q", q => q.QuestionCode),
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question == null)
                throw new QuestionNotFoundException();

            if (request.TopicId.HasValue)
            {
                var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);
                if (topic == null || topic.DeletedAt.HasValue)
                    throw new TopicNotFoundException();
            }

            var currentUser = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var isAdmin = _httpContextAccessor.HttpContext?.User?.IsInRole(nameof(Role.Admin)) ?? false;

            if (!isAdmin && !string.Equals(question.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("You are not allowed to edit this question.");

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

            if (question.Status == StatusQEM.Removed && request.Status != StatusQEM.Removed)
                question.DeletedAt = null;

            question.TopicId = request.TopicId;
            question.Content = request.Content;
            question.Level = request.Level;
            question.Type = request.Type;
            question.Answer1 = request.Answer1;
            question.Answer2 = request.Answer2;
            question.Answer3 = request.Answer3;
            question.Answer4 = request.Answer4;
            question.Answer5 = request.Answer5;
            question.Answer6 = request.Answer6;
            question.Grade = request.Grade;
            question.Status = request.Status;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }

    public class UpdateQuestionStatusCommandHandler : IRequestHandler<UpdateQuestionStatusCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionStatusCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(UpdateQuestionStatusCommand request, CancellationToken cancellationToken)
        {
            var questions = new List<Question>();

            foreach (var id in request.Ids)
            {
                var question = await _questionRepository.GetByIdAsync(id);
                if (question == null)
                    throw new QuestionNotFoundException();

                if (!CheckCanChangeStatus.CanChangeStatus(question.Status, request.Status))
                {
                    throw new InvalidOperationException(
                        $"Không thể chuyển câu hỏi {question.QuestionCode} từ {question.Status} sang {request.Status}"
                    );
                }

                questions.Add(question);
            }

            foreach (var question in questions)
            {
                question.Status = request.Status;
                await _questionRepository.UpdateAsync(question);
            }

            return Unit.Value;
        }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _questionRepository = questionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question == null || question.DeletedAt.HasValue)
                throw new QuestionNotFoundException();

            var currentUser = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var isAdmin = _httpContextAccessor.HttpContext?.User?.IsInRole(nameof(Role.Admin)) ?? false;

            if (!isAdmin && !string.Equals(question.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("You are not allowed to delete this question.");

            question.DeletedAt = DateTime.UtcNow;
            question.Status = StatusQEM.Removed;

            await _questionRepository.UpdateAsync(question);
            return Unit.Value;
        }
    }
}
