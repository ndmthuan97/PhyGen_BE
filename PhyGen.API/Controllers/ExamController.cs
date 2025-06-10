using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : BaseController<ExamController>
    {
        public ExamController(IMediator mediator, ILogger<ExamController> logger)
            : base(mediator, logger) { }
        
         [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ExamResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllExams()
        {
            var request = new GetAllExamsQuery();
            return await ExecuteAsync<GetAllExamsQuery, List<ExamResponse>>(request);
        }

        [HttpGet("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamById(Guid examId)
        {
            var request = new GetExamByIdQuery(examId);
            return await ExecuteAsync<GetExamByIdQuery, ExamResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCommand>(request);
            return await ExecuteAsync<CreateExamCommand, Guid>(command);
        }

        [HttpPut("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExam(Guid examId, [FromBody] UpdateExamRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCommand>(request);
            command.ExamId = examId;
            return await ExecuteAsync<UpdateExamCommand, Unit>(command);
        }

        [HttpDelete("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteExam(Guid examId)
        {
            var command = new DeleteExamCommand { ExamId = examId };
            return await ExecuteAsync<DeleteExamCommand, Unit>(command);
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
