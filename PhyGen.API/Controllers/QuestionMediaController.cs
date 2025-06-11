using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
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
        public async Task<IActionResult> GetById(Guid questionMediaId)
        {
            var request = new GetQuestionMediaByIdQuery(questionMediaId);
            return await ExecuteAsync<GetQuestionMediaByIdQuery, QuestionMediaResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateQuestionMediaRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateQuestionMediaCommand>(request);
            return await ExecuteAsync<CreateQuestionMediaCommand, Guid>(command);
        }

        [HttpPut("{questionMediaId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid questionMediaId, [FromBody] UpdateQuestionMediaRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionMediaCommand>(request);
            command.QuestionMediaId = questionMediaId;
            return await ExecuteAsync<UpdateQuestionMediaCommand, Unit>(command);
        }

        private IActionResult HandleNullRequest()
        {
            return BadRequest(new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                Errors = ["The request body does not contain required fields"]
            });
        }
    }
}
