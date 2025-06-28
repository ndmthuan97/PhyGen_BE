using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Sections.Commands;
using PhyGen.Application.Sections.Queries;
using PhyGen.Application.Sections.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionController : BaseController<SectionController>
    {
        public SectionController(IMediator mediator, ILogger<SectionController> logger)
            : base(mediator, logger) { }

        [HttpGet("{sectionId}")]
        [ProducesResponseType(typeof(ApiResponse<SectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSectionById(Guid sectionId)
        {
            var request = new GetSectionByIdQuery(sectionId);
            return await ExecuteAsync<GetSectionByIdQuery, SectionResponse>(request);
        }

        [HttpGet("exam/{examId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SectionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSectionsByExamId(Guid examId)
        {
            var request = new GetSectionsByExamIdQuery(examId);
            return await ExecuteAsync<GetSectionsByExamIdQuery, IEnumerable<SectionResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionRequest request)
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
            var command = AppMapper<CoreMappingProfile>.Mapper.Map<CreateSectionCommand>(request);
            return await ExecuteAsync<CreateSectionCommand, SectionResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateSectionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or the IDs do not match"]
                });
            }
            var command = AppMapper<CoreMappingProfile>.Mapper.Map<UpdateSectionCommand>(request);
            return await ExecuteAsync<UpdateSectionCommand, Unit>(command);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody] DeleteSectionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or the IDs do not match"]
                });
            }
            var command = AppMapper<CoreMappingProfile>.Mapper.Map<DeleteSectionCommand>(request);
            return await ExecuteAsync<DeleteSectionCommand, Unit>(command);
        }
    }
}
