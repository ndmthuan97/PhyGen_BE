using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ExamQuestions.Commands;
using PhyGen.Application.ExamQuestions.Queries;
using PhyGen.Application.ExamQuestions.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/examquestions")]
    [ApiController]
    public class ExamQuestionController : BaseController<ExamQuestionController>
    {
        public ExamQuestionController(IMediator mediator, ILogger<ExamQuestionController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ExamQuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetAllExamQuestionsQuery();
            return await ExecuteAsync<GetAllExamQuestionsQuery, List<ExamQuestionResponse>>(request);
        }

        [HttpGet("{examQuestionId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamQuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(Guid examQuestionId)
        {
            var request = new GetExamQuestionByIdQuery(examQuestionId);
            return await ExecuteAsync<GetExamQuestionByIdQuery, ExamQuestionResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateExamQuestionRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamQuestionCommand>(request);
            return await ExecuteAsync<CreateExamQuestionCommand, Guid>(command);
        }

        [HttpPut("{examQuestionId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid examQuestionId, [FromBody] UpdateExamQuestionRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamQuestionCommand>(request);
            command.ExamQuestionId = examQuestionId;
            return await ExecuteAsync<UpdateExamQuestionCommand, Unit>(command);
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
