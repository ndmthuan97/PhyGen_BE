using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.Application.Mapping;
using PhyGen.Application.QuestionSections.Commands;
using PhyGen.Application.QuestionSections.Queries;
using PhyGen.Application.QuestionSections.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/questionsections")]
    [ApiController]
    public class QuestionSectionController : BaseController<QuestionSectionController>
    {
        public QuestionSectionController(IMediator mediator, ILogger<QuestionSectionController> logger)
            : base(mediator, logger) { }

        [HttpGet("{questionSectionId}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionSectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionSectionById(Guid questionSectionId)
        {
            var request = new GetQuestionSectionByIdQuery(questionSectionId);
            return await ExecuteAsync<GetQuestionSectionByIdQuery, QuestionSectionResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<QuestionSectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionSectionByQuestionIdAndSectionId(Guid questionId, Guid sectionId)
        {
            var request = new GetQuestionSectionsByQuestionIdAndSectionIdQuery(questionId, sectionId);
            return await ExecuteAsync<GetQuestionSectionsByQuestionIdAndSectionIdQuery, QuestionSectionResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionSectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateQuestionSection([FromBody] CreateQuestionSectionCommand request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateQuestionSectionCommand>(request);
            return await ExecuteAsync<CreateQuestionSectionCommand, QuestionSectionResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestionSection([FromBody] UpdateQuestionSectionCommand request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionSectionCommand>(request);
            return await ExecuteAsync<UpdateQuestionSectionCommand, Unit>(command);
        }
    }
}
