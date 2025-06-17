using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ContentItemExamCategories.Commands;
using PhyGen.Application.ContentItemExamCategories.Queries;
using PhyGen.Application.ContentItemExamCategories.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/contentitemexamcategories")]
    [ApiController]
    public class ContentItemExamCategoryController : BaseController<ContentItemExamCategoryController>
    {
        public ContentItemExamCategoryController(IMediator mediator, ILogger<ContentItemExamCategoryController> logger)
            : base(mediator, logger) { }

        [HttpGet("{contentItemExamCategoryId}")]
        [ProducesResponseType(typeof(ApiResponse<ContentItemExamCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentItemExamCategoryById(Guid contentItemExamCategoryId)
        {
            var request = new GetContentItemExamCategoryByIdQuery(contentItemExamCategoryId);
            return await ExecuteAsync<GetContentItemExamCategoryByIdQuery, ContentItemExamCategoryResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ContentItemExamCategoryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContentItemExamCategoriesByContentItemIdAndExamCategoryId([FromQuery] Guid contentItemId, [FromQuery] Guid examCategoryId)
        {
            var request = new GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery(contentItemId, examCategoryId);
            return await ExecuteAsync<GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery, List<ContentItemExamCategoryResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ContentItemExamCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateContentItemExamCategory([FromBody] CreateContentItemExamCategoryRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateContentItemExamCategoryCommand>(request);
            return await ExecuteAsync<CreateContentItemExamCategoryCommand, Guid>(command);
        }

        [HttpPut("{contentItemExamCategoryId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContentItemExamCategory(Guid contentItemExamCategoryId, [FromBody] UpdateContentItemExamCategoryRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateContentItemExamCategoryCommand>(request);
            return await ExecuteAsync<UpdateContentItemExamCategoryCommand, Unit>(command);
        }
    }
}
