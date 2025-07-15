using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Topics.Commands;
using PhyGen.Application.Topics.Queries;
using PhyGen.Application.Topics.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Domain.Specs;
using Microsoft.AspNetCore.Authorization;

namespace PhyGen.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : BaseController<TopicController>
    {
        public TopicController(IMediator mediator, ILogger<TopicController> logger)
            : base(mediator, logger) { }

        [HttpGet("{topicId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<TopicResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTopicById(Guid topicId)
        {
            var request = new GetTopicByIdQuery(topicId);
            return await ExecuteAsync<GetTopicByIdQuery, TopicResponse>(request);
        }

        [HttpGet("chapter")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<List<TopicResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTopicsByChapterId([FromQuery] TopicSpecParam param)
        {
            var request = new GetTopicsByChapterIdQuery(param);
            return await ExecuteAsync<GetTopicsByChapterIdQuery, Pagination<TopicResponse>>(request);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<TopicResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateTopicCommand>(request);
            return await ExecuteAsync<CreateTopicCommand, Guid>(command);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopicRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateTopicCommand>(request);
            return await ExecuteAsync<UpdateTopicCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteTopic([FromBody] DeleteTopicRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteTopicCommand>(request);
            return await ExecuteAsync<DeleteTopicCommand, Unit>(command);
        }
    }
}
