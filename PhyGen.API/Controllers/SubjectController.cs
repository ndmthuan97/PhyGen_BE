using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Subjects.Commands;
using PhyGen.Application.Subjects.Queries;
using PhyGen.Application.Subjects.Responses;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectController : BaseController<SubjectController>
    {
        public SubjectController(IMediator mediator, ILogger<SubjectController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<SubjectResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllSubjects()
        {
            var request = new GetAllSubjectsQuery();
            return await ExecuteAsync<GetAllSubjectsQuery, List<SubjectResponse>>(request);
        }

        [HttpGet("{subjectId}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubjectById(Guid subjectId)
        {
            var request = new GetSubjectByIdQuery(subjectId);
            return await ExecuteAsync<GetSubjectByIdQuery, SubjectResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateSubjectCommand>(request);
            return await ExecuteAsync<CreateSubjectCommand, Guid>(command);
        }

        [HttpPut("{subjectId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSubject(Guid subjectId, [FromBody] UpdateSubjectRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateSubjectCommand>(request);
            return await ExecuteAsync<UpdateSubjectCommand, Unit>(command);
        }
    }
}
