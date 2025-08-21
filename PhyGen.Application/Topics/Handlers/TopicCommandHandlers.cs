using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Topics.Commands;
using PhyGen.Application.Topics.Exceptions;
using PhyGen.Application.Topics.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Handlers
{
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, TopicResponse>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateTopicCommandHandler(ITopicRepository topicRepository, IChapterRepository chapterRepository)
        {
            _topicRepository = topicRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<TopicResponse> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.ChapterId);
            if (chapter == null || chapter.DeletedAt.HasValue)
                throw new ChapterNotFoundException();

            if (await _topicRepository.AlreadyExistAsync(t =>
                t.ChapterId == request.ChapterId &&
                t.Name.ToLower() == request.Name.ToLower() &&
                t.DeletedAt == null
                ))
                throw new TopicAlreadyExistException();

            var topic = new Topic
            {
                ChapterId = request.ChapterId,
                Name = request.Name,
                TopicCode = await _topicRepository.GenerateCodeAsync<Topic>("T", t => t.TopicCode),
            };

            await _topicRepository.AddAsync(topic);
            return AppMapper<CoreMappingProfile>.Mapper.Map<TopicResponse>(topic);
        }
    }

    public class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Unit>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IChapterRepository _chapterRepository;
        public UpdateTopicCommandHandler(ITopicRepository topicRepository, IChapterRepository chapterRepository)
        {
            _topicRepository = topicRepository;
            _chapterRepository = chapterRepository;
        }
        public async Task<Unit> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {

            var topic = await _topicRepository.GetByIdAsync(request.Id) ?? throw new TopicNotFoundException();

            var chapter = await _chapterRepository.GetByIdAsync(request.ChapterId);
            if (chapter == null || chapter.DeletedAt.HasValue)
                throw new ChapterNotFoundException();

            if (await _topicRepository.AlreadyExistAsync(t =>
                t.ChapterId == request.ChapterId &&
                t.Name.ToLower() == request.Name.ToLower() &&
                t.DeletedAt == null
                ))
                throw new TopicAlreadyExistException();

            topic.ChapterId = request.ChapterId;
            topic.Name = request.Name;

            await _topicRepository.UpdateAsync(topic);
            return Unit.Value;
        }
    }

    public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Unit>
    {
        private readonly ITopicRepository _topicRepository;
        public DeleteTopicCommandHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }
        public async Task<Unit> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.Id) ?? throw new TopicNotFoundException();

            topic.DeletedAt = DateTime.UtcNow;

            await _topicRepository.UpdateAsync(topic);
            return Unit.Value;
        }
    }
}
