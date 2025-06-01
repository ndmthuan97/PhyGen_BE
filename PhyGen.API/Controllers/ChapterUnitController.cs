using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.ChapterUnits;
using PhyGen.Application.ChapterUnits.Commands;
using PhyGen.Application.ChapterUnits.Queries;
using PhyGen.Application.ChapterUnits.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/chapterunits")]
    [ApiController]
    public class ChapterUnitController : BaseController<ChapterUnitController>
    {
        public ChapterUnitController(IMediator mediator, ILogger<ChapterUnitController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ChapterUnitResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllChapterUnits()
        {
            var request = new GetAllChapterUnitsQuery();
            return await ExecuteAsync<GetAllChapterUnitsQuery, List<ChapterUnitResponse>>(request);
        }

        [HttpGet("{chapterUnitId}")]
        [ProducesResponseType(typeof(ApiResponse<ChapterUnitResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChapterUnitById(Guid chapterUnitId)
        {
            var request = new GetChapterUnitByIdQuery(chapterUnitId);
            return await ExecuteAsync<GetChapterUnitByIdQuery, ChapterUnitResponse>(request);
        }

        [HttpGet("chapters/{chapterId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ChapterUnitResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChapterUnitsByChapterId(Guid chapterId)
        {
            var request = new GetChapterUnitsByChapterIdQuery(chapterId);
            return await ExecuteAsync<GetChapterUnitsByChapterIdQuery, List<ChapterUnitResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateChapterUnit([FromBody] CreateChapterUnitRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateChapterUnitCommand>(request);
            return await ExecuteAsync<CreateChapterUnitCommand, Guid>(command);
        }

        [HttpPut("{chapterUnitId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateChapterUnit(Guid chapterUnitId, [FromBody] UpdateChapterUnitRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateChapterUnitCommand>(request);
            command.ChapterUnitId = chapterUnitId;
            return await ExecuteAsync<UpdateChapterUnitCommand, Unit>(command);
        }
    }
}
