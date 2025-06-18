using MediatR;
using PhyGen.Application.Chapters.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Topics.Exceptions;
using PhyGen.Application.Topics.Queries;
using PhyGen.Application.Topics.Responses;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Handlers
{
    public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, TopicResponse>
    {
        private readonly ITopicRepository _topicRepository;
        public GetTopicByIdQueryHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }
        public async Task<TopicResponse> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.Id) ?? throw new TopicNotFoundException();
            
            return AppMapper<CoreMappingProfile>.Mapper.Map<TopicResponse>(topic);
        }
    }

    public class GetTopicsByChapterIdQueryHandler : IRequestHandler<GetTopicsByChapterIdQuery, Pagination<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IChapterRepository _chapterRepository;

        public GetTopicsByChapterIdQueryHandler(ITopicRepository topicRepository, IChapterRepository chapterRepository)
        {
            _topicRepository = topicRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Pagination<TopicResponse>> Handle(GetTopicsByChapterIdQuery request, CancellationToken cancellationToken)
        {
            var topics = await _topicRepository.GetTopicsByChapterAsync(request.TopicSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<TopicResponse>>(topics);
        }
    }
}
