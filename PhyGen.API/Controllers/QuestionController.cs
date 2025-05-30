using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class QuestionController : BaseController<QuestionController>
    {
        public QuestionController(IMediator mediator, ILogger<QuestionController> logger)
            : base(mediator, logger) { }

        // GET: api/questions
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllQuestions()
        {
            var request = new GetAllQuestionsQuery();
            return await ExecuteAsync<GetAllQuestionsQuery, List<QuestionResponse>>(request);
        }

        // GET: api/questions/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var request = new GetQuestionByIdQuery(id);
            return await ExecuteAsync<GetQuestionByIdQuery, QuestionResponse>(request);
        }

        // POST: api/questions
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
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateQuestionCommand>(request);
            return await ExecuteAsync<CreateQuestionCommand, Guid>(command);
        }

        // PUT: api/questions/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionRequest request)
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

            request.Id = id;
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionCommand>(request);
            return await ExecuteAsync<UpdateQuestionCommand, Unit>(command);
        }

        // DELETE: api/questions/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var command = new DeleteQuestionCommand(id);
            return await ExecuteAsync<DeleteQuestionCommand, Unit>(command);
        }
    }
}
