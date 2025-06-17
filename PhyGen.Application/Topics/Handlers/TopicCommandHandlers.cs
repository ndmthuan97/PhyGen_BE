using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Topics.Commands;
using PhyGen.Application.Topics.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Handlers
{
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IChapterRepository _chapterRepository;

        public CreateTopicCommandHandler(ITopicRepository topicRepository, IChapterRepository chapterRepository)
        {
            _topicRepository = topicRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            if (await _topicRepository.GetTopicByChapterIdAndNameAsync(request.ChapterId, request.Name) != null)
                throw new TopicSameNameException();

            var topic = new Topic
            {
                ChapterId = request.ChapterId,
                Name = request.Name,
                OrderNo = request.OrderNo
            };

            await _topicRepository.AddAsync(topic);
            return topic.Id;
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
            if (await _chapterRepository.GetByIdAsync(request.ChapterId) == null)
                throw new ChapterNotFoundException();

            var topic = await _topicRepository.GetByIdAsync(request.Id) ?? throw new TopicNotFoundException();

            if (await _topicRepository.GetTopicByChapterIdAndNameAsync(request.ChapterId, request.Name) != null &&
                topic.Name != request.Name)
                throw new TopicSameNameException();

            topic.ChapterId = request.ChapterId;
            topic.Name = request.Name;
            topic.OrderNo = request.OrderNo;

            await _topicRepository.UpdateAsync(topic);
            return Unit.Value;
        }
    }
}
