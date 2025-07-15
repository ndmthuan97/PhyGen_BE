using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.SubjectBooks.Commands;
using PhyGen.Application.SubjectBooks.Queries;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Domain.Specs;
using Microsoft.AspNetCore.Authorization;

namespace PhyGen.API.Controllers
{
    [Route("api/subjectbooks")]
    [ApiController]
    public class SubjectBookController : BaseController<SubjectBookController>
    {
        public SubjectBookController(IMediator mediator, ILogger<SubjectBookController> logger)
            : base(mediator, logger) { }

        [HttpGet("{subjectBookId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<SubjectBookResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubjectBookById(Guid subjectBookId)
        {
            var request = new GetSubjectBookByIdQuery(subjectBookId);
            return await ExecuteAsync<GetSubjectBookByIdQuery, SubjectBookResponse>(request);
        }

        [HttpGet("subject")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<List<SubjectBookResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubjectBooksBySubjectId([FromQuery] SubjectBookSpecParam param)
        {
            var request = new GetSubjectBooksBySubjectIdQuery(param);
            return await ExecuteAsync<GetSubjectBooksBySubjectIdQuery, Pagination<SubjectBookResponse>>(request);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<SubjectBookResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSubjectBook([FromBody] CreateSubjectBookRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateSubjectBookCommand>(request);
            return await ExecuteAsync<CreateSubjectBookCommand, SubjectBookResponse>(command);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSubjectBook([FromBody] UpdateSubjectBookRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateSubjectBookCommand>(request);
            return await ExecuteAsync<UpdateSubjectBookCommand, Unit>(command);
        }
        
        [HttpDelete]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteSubjectBook([FromBody] DeleteSubjectBookRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteSubjectBookCommand>(request);
            return await ExecuteAsync<DeleteSubjectBookCommand, Unit>(command);
        }
    }
}
