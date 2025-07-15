using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAllQuestions([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var query = new GetQuestionsQuery(questionSpecParam);
            return await ExecuteAsync<GetQuestionsQuery, Pagination<QuestionResponse>>(query);
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionById(Guid questionId)
        {
            var request = new GetQuestionByIdQuery(questionId);
            return await ExecuteAsync<GetQuestionByIdQuery, QuestionResponse>(request);
        }

        [HttpGet("topic")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByTopicId([FromQuery] QuestionByTopicSpecParam param)
        {
            var request = new GetQuestionsByTopicIdQuery(param);
            return await ExecuteAsync<GetQuestionsByTopicIdQuery, Pagination<QuestionResponse>>(request);
        }

        [HttpGet("level&type")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByLevelAndType([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var request = new GetQuestionsByLevelAndTypeQuery(questionSpecParam);
            return await ExecuteAsync<GetQuestionsByLevelAndTypeQuery, Pagination<QuestionResponse>>(request);
        }

        [HttpGet("grade")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByGrade([FromQuery] QuestionByGradeSpecParam questionGradeParam)
        {
            var request = new GetQuestionsByGradeQuery(questionGradeParam);
            return await ExecuteAsync<GetQuestionsByGradeQuery, Pagination<QuestionResponse>>(request);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
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

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new UserNotFoundException());

            command.CreatedBy = userEmail;

            return await ExecuteAsync<CreateQuestionCommand, QuestionResponse>(command);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest request)
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

            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionCommand>(request);
            return await ExecuteAsync<UpdateQuestionCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteQuestion([FromBody] DeleteQuestionRequest request)
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

            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteQuestionCommand>(request);
            return await ExecuteAsync<DeleteQuestionCommand, Unit>(command);
        }
    }
}