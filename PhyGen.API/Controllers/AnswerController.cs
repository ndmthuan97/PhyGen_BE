using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.Answers;
using PhyGen.Application.Answers.Commands;
using PhyGen.Application.Answers.Queries;
using PhyGen.Application.Answers.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswerController : BaseController<AnswerController>
    {
        public AnswerController(IMediator mediator, ILogger<AnswerController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AnswerResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAnswers()
        {
            var query = new GetAllAnswersQuery();
            return await ExecuteAsync<GetAllAnswersQuery, List<AnswerResponse>>(query);
        }

        [HttpGet("{answerId}")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAnswerById(Guid answerId)
        {
            var query = new GetAnswerByIdQuery(answerId);
            return await ExecuteAsync<GetAnswerByIdQuery, AnswerResponse>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateAnswerCommand>(request);
            return await ExecuteAsync<CreateAnswerCommand, Guid>(command);
        }

        [HttpPut("{answerId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAnswer(Guid answerId, [FromBody] UpdateAnswerRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateAnswerCommand>(request);
            return await ExecuteAsync<UpdateAnswerCommand, Unit>(command);
        }
    }
}
