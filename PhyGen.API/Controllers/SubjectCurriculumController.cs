using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.SubjectCurriculums.Commands;
using PhyGen.Application.SubjectCurriculums.Queries;
using PhyGen.Application.SubjectCurriculums.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/subjectcurriculums")]
    [ApiController]
    public class SubjectCurriculumController : BaseController<SubjectCurriculumController>
    {
        public SubjectCurriculumController(IMediator mediator, ILogger<SubjectCurriculumController> logger)
            : base(mediator, logger)
        {
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(ApiResponse<List<SubjectCurriculumResponse>>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetAllSubjectCurriculums()
        //{
        //    var request = new GetAllSubjectCurriculumsQuery();
        //    return await ExecuteAsync<GetAllSubjectCurriculumsQuery, List<SubjectCurriculumResponse>>(request);
        //}

        [HttpGet("{subjectCurriculumId}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectCurriculumResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubjectCurriculumById(Guid subjectCurriculumId)
        {
            var request = new GetSubjectCurriculumByIdQuery(subjectCurriculumId);
            return await ExecuteAsync<GetSubjectCurriculumByIdQuery, SubjectCurriculumResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<SubjectCurriculumResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubjectCurriculumBySubjectIdAndCurriculumId([FromQuery] Guid subjectId, [FromQuery] Guid curriculumId)
        {
            var request = new GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery(subjectId, curriculumId);
            return await ExecuteAsync<GetSubjectCurriculumBySubjectIdAndCurriculumIdQuery, SubjectCurriculumResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SubjectCurriculumResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSubjectCurriculum([FromBody] CreateSubjectCurriculumRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateSubjectCurriculumCommand>(request);
            return await ExecuteAsync<CreateSubjectCurriculumCommand, Guid>(command);
        }

        [HttpPut("{subjectCurriculumId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSubjectCurriculum(Guid subjectCurriculumId, [FromBody] UpdateSubjectCurriculumRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateSubjectCurriculumCommand>(request);
            return await ExecuteAsync<UpdateSubjectCurriculumCommand, Unit>(command);
        }
    }
}
