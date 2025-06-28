using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Application.ExamCategories.Responses;
using PhyGen.Application.ExamCategories.Queries;
using PhyGen.Application.ExamCategories.Commands;

namespace PhyGen.API.Controllers
{
    [Route("api/examcategories")]
    [ApiController]
    public class ExamCategoryController : BaseController<ExamCategoryController>
    {
        public ExamCategoryController(IMediator mediator, ILogger<ExamCategoryController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ExamCategoryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllExamCategories()
        {
            var request = new GetAllExamCategoriesQuery();
            return await ExecuteAsync<GetAllExamCategoriesQuery, List<ExamCategoryResponse>>(request);
        }

        [HttpGet("{examCategoryId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamCategoryById(Guid examCategoryId)
        {
            var request = new GetExamCategoryByIdQuery(examCategoryId);
            return await ExecuteAsync<GetExamCategoryByIdQuery, ExamCategoryResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExamCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExamCategory([FromBody] CreateExamCategoryRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCategoryCommand>(request);
            return await ExecuteAsync<CreateExamCategoryCommand, ExamCategoryResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExamCategory([FromBody] UpdateExamCategoryRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCategoryCommand>(request);
            return await ExecuteAsync<UpdateExamCategoryCommand, Unit>(command);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteExamCategory([FromBody] DeleteExamCategoryRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteExamCategoryCommand>(request);
            return await ExecuteAsync<DeleteExamCategoryCommand, Unit>(command);
        }
    }
}
