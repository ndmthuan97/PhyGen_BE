using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Specs;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : BaseController<ExamController>
    {
        public ExamController(IMediator mediator, ILogger<ExamController> logger)
            : base(mediator, logger) { }

        [HttpGet("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamById(Guid examId)
        {
            var query = new GetExamByIdQuery(examId);
            return await ExecuteAsync<GetExamByIdQuery, ExamResponse>(query);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Pagination<ExamResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllExams([FromQuery] ExamSpecParam param)
        {
            var query = new GetExamsQuery(param);
            return await ExecuteAsync<GetExamsQuery, Pagination<ExamResponse>>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCommand>(request);
            return await ExecuteAsync<CreateExamCommand, ExamResponse>(command);
        }

        [HttpPut("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExam(Guid examId, [FromBody] UpdateExamRequest request)
        {
            if (request == null || request.Id != examId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCommand>(request);
            return await ExecuteAsync<UpdateExamCommand, Unit>(command);
        }

        [HttpDelete("{examId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteExam(Guid examId, [FromBody] DeleteExamRequest request)
        {
            if (request == null || request.Id != examId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteExamCommand>(request);
            return await ExecuteAsync<DeleteExamCommand, Unit>(command);
        }
    }
}
