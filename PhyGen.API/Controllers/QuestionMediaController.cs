using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.QuestionMedias.Commands;
using PhyGen.Application.QuestionMedias.Queries;
using PhyGen.Application.QuestionMedias.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/questionmedias")]
    [ApiController]
    public class QuestionMediaController : BaseController<QuestionMediaController>
    {
        public QuestionMediaController(IMediator mediator, ILogger<QuestionMediaController> logger)
            : base(mediator, logger) { }

        [HttpGet("{questionMediaId}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionMediaResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionMediaById(Guid questionMediaId)
        {
            var request = new GetQuestionMediaByIdQuery(questionMediaId);
            return await ExecuteAsync<GetQuestionMediaByIdQuery, QuestionMediaResponse>(request);
        }

        [HttpGet("question/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionMediaResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionMediasByQuestionId(Guid questionId)
        {
            var request = new GetQuestionMediasByQuestionIdQuery(questionId);
            return await ExecuteAsync<GetQuestionMediasByQuestionIdQuery, IEnumerable<QuestionMediaResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionMediaResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateQuestionMedia([FromBody] CreateQuestionMediaRequest request)
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

            var command = AppMapper<CoreMappingProfile>.Mapper.Map<CreateQuestionMediaCommand>(request);
            return await ExecuteAsync<CreateQuestionMediaCommand, QuestionMediaResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionMediaRequest request)
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

            var command = AppMapper<CoreMappingProfile>.Mapper.Map<UpdateQuestionMediaCommand>(request);
            return await ExecuteAsync<UpdateQuestionMediaCommand, Unit>(command);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody] DeleteQuestionMediaRequest request)
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

            var command = AppMapper<CoreMappingProfile>.Mapper.Map<DeleteQuestionMediaCommand>(request);
            return await ExecuteAsync<DeleteQuestionMediaCommand, Unit>(command);
        }
    }
}
