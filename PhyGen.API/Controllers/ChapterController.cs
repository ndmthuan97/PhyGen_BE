using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Chapters.Commands;
using PhyGen.Application.Chapters.Queries;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Domain.Specs;
using Microsoft.AspNetCore.Authorization;

namespace PhyGen.API.Controllers
{
    [Route("api/chapters")]
    [ApiController]
    public class ChapterController : BaseController<ChapterController>
    {
        public ChapterController(IMediator mediator, ILogger<ChapterController> logger)
            : base(mediator, logger) { }

        [HttpGet("{chapterId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<ChapterResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChapterById(Guid chapterId)
        {
            var request = new GetChapterByIdQuery(chapterId);
            return await ExecuteAsync<GetChapterByIdQuery, ChapterResponse>(request);
        }

        [HttpGet("subjectbook")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<List<ChapterResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChaptersBySubjectBookId([FromQuery] ChapterSpecParam chapterSpecParam)
        {
            var request = new GetChaptersBySubjectBookIdQuery(chapterSpecParam);
            return await ExecuteAsync<GetChaptersBySubjectBookIdQuery, Pagination<ChapterResponse>>(request);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<ChapterResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateChapter([FromBody] CreateChapterRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateChapterCommand>(request);
            return await ExecuteAsync<CreateChapterCommand, ChapterResponse>(command);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateChapter([FromBody] UpdateChapterRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateChapterCommand>(request);
            return await ExecuteAsync<UpdateChapterCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteChapter([FromBody] DeleteChapterRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteChapterCommand>(request);
            return await ExecuteAsync<DeleteChapterCommand, Unit>(command);
        }
    }
}
