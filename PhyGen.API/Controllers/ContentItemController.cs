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
            var request = new GetContentItemByIdQuery(contentItemId);
            return await ExecuteAsync<GetContentItemByIdQuery, ContentItemResponse>(request);
        }

        [HttpGet("contentflow/{contentFlowId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ContentItemResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentItemsByContentFlowId(Guid contentFlowId)
        {
            var request = new GetContentItemsByContentFlowIdQuery(contentFlowId);
            return await ExecuteAsync<GetContentItemsByContentFlowIdQuery, List<ContentItemResponse>>(request);
        }

        [HttpPost]
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

        [HttpPut("{contentItemId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContentItem(Guid contentItemId, [FromBody] UpdateContentItemRequest request)
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
    }
}
