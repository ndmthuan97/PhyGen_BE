using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Queries;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.Users.Exceptions;
using System.Security.Claims;

namespace PhyGen.API.Controllers
{
    [Route("api/contentflows")]
    [ApiController]
    public class ContentFlowController : BaseController<ContentFlowController>
    {
        public ContentFlowController(IMediator mediator, ILogger<ContentFlowController> logger)
            : base(mediator, logger) { }

        [HttpGet("{contentFlowId}")]
        [ProducesResponseType(typeof(ApiResponse<ContentFlowResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentFlowById(Guid contentFlowId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var request = new GetContentFlowByIdQuery(contentFlowId);
            return await ExecuteAsync<GetContentFlowByIdQuery, ContentFlowResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ContentFlowResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentFlowsByCurriculumIdAndSubjectId([FromQuery] Guid curriculumId, [FromQuery] Guid subjectId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var request = new GetContentFlowsByCurriculumIdAndSubjectIdQuery(curriculumId, subjectId);
            return await ExecuteAsync<GetContentFlowsByCurriculumIdAndSubjectIdQuery, List<ContentFlowResponse>>(request);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<ContentFlowResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateContentFlow([FromBody] CreateContentFlowRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateContentFlowCommand>(request);
            return await ExecuteAsync<CreateContentFlowCommand, ContentFlowResponse>(command);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContentFlow([FromBody] UpdateContentFlowRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateContentFlowCommand>(request);
            return await ExecuteAsync<UpdateContentFlowCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteContentFlow([FromBody] DeleteContentFlowRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteContentFlowCommand>(request);
            return await ExecuteAsync<DeleteContentFlowCommand, Unit>(command);
        }
    }
}
