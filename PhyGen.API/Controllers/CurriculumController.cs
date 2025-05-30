using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.Curriculums;
using PhyGen.Application.Curriculums.Commands;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Application.Curriculums.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/curriculums")]
    [ApiController]
    public class CurriculumController : BaseController<CurriculumController>
    {
        public CurriculumController(IMediator mediator, ILogger<CurriculumController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<CurriculumResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCurriculums()
        {
            var request = new GetAllCurriculumsQuery();
            return await ExecuteAsync<GetAllCurriculumsQuery, List<CurriculumResponse>>(request);
        }

        [HttpGet("{curriculumId}")]
        [ProducesResponseType(typeof(ApiResponse<CurriculumResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurriculumById(Guid curriculumId)
        {
            var request = new GetCurriculumByIdQuery(curriculumId);
            return await ExecuteAsync<GetCurriculumByIdQuery, CurriculumResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CurriculumResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCurriculum([FromBody] CreateCurriculumRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateCurriculumCommand>(request);
            return await ExecuteAsync<CreateCurriculumCommand, Guid>(command);
        }

        [HttpPut("{curriculumId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCurriculum(Guid curriculumId, [FromBody] UpdateCurriculumRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateCurriculumCommand>(request);
            return await ExecuteAsync<UpdateCurriculumCommand, Unit>(command);
        }
    }
}
