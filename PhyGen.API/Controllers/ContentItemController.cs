using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ContentItems.Commands;
using PhyGen.Application.ContentItems.Queries;
using PhyGen.Application.ContentItems.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.Users.Exceptions;
using System.Security.Claims;

namespace PhyGen.API.Controllers
{
    [Route("api/contentitems")]
    [ApiController]
    public class ContentItemController : BaseController<ContentItemController>
    {
        public ContentItemController(IMediator mediator, ILogger<ContentItemController> logger)
            : base(mediator, logger) { }

        [HttpGet("{contentItemId}")]
        [ProducesResponseType(typeof(ApiResponse<ContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentItemById(Guid contentItemId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var request = new GetContentItemByIdQuery(contentItemId);
            return await ExecuteAsync<GetContentItemByIdQuery, ContentItemResponse>(request);
        }

        [HttpGet("contentflow/{contentFlowId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ContentItemResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentItemsByContentFlowId(Guid contentFlowId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var request = new GetContentItemsByContentFlowIdQuery(contentFlowId);
            return await ExecuteAsync<GetContentItemsByContentFlowIdQuery, List<ContentItemResponse>>(request);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<ContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateContentItem([FromBody] CreateContentItemRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateContentItemCommand>(request);
            return await ExecuteAsync<CreateContentItemCommand, ContentItemResponse>(command);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContentItem([FromBody] UpdateContentItemRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateContentItemCommand>(request);
            return await ExecuteAsync<UpdateContentItemCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteContentItem([FromBody] DeleteContentItemRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteContentItemCommand>(request);
            return await ExecuteAsync<DeleteContentItemCommand, Unit>(command);
        }
    }
}
