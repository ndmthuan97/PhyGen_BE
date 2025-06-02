using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.Questions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : BaseController<QuestionController>
    {
        public QuestionController(IMediator mediator, ILogger<QuestionController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllQuestions()
        {
            var query = new GetAllQuestionsQuery();
            return await ExecuteAsync<GetAllQuestionsQuery, List<QuestionResponse>>(query);
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionById(Guid questionId)
        {
            var query = new GetQuestionByIdQuery(questionId);
            return await ExecuteAsync<GetQuestionByIdQuery, QuestionResponse>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields."]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateQuestionCommand>(request);
            return await ExecuteAsync<CreateQuestionCommand, Guid>(command);
        }

        [HttpPut("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestion(Guid questionId, [FromBody] UpdateQuestionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields."]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionCommand>(request);
            return await ExecuteAsync<UpdateQuestionCommand, Unit>(command);
        }
    }
}
