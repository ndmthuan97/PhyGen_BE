using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Application.ExamCategoryChapters.Responses;
using PhyGen.Application.ExamCategoryChapters.Queries;
using PhyGen.Application.ExamCategoryChapters.Commands;

namespace PhyGen.API.Controllers
{
    [Route("api/examcategorychapters")]
    [ApiController]
    public class ExamCategoryChapterController : BaseController<ExamCategoryChapterController>
    {
        public ExamCategoryChapterController(IMediator mediator, ILogger<ExamCategoryChapterController> logger)
            : base(mediator, logger) { }

        [HttpGet("{examCategoryChapterId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamCategoryChapterResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamCategoryChapterById(Guid examCategoryChapterId)
        {
            var request = new GetExamCategoryChapterByIdQuery(examCategoryChapterId);
            return await ExecuteAsync<GetExamCategoryChapterByIdQuery, ExamCategoryChapterResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ExamCategoryChapterResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamCategoryChapterByExamCategoryIdAndChapterId([FromQuery] Guid examCategoryId, [FromQuery] Guid chapterId)
        {
            var request = new GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery(examCategoryId, chapterId);
            return await ExecuteAsync<GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery, List<ExamCategoryChapterResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExamCategoryChapterResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExamCategoryChapter([FromBody] CreateExamCategoryChapterRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCategoryChapterCommand>(request);
            return await ExecuteAsync<CreateExamCategoryChapterCommand, Guid>(command);
        }

        [HttpPut("{examCategoryChapterId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExamCategoryChapter(Guid examCategoryChapterId, [FromBody] UpdateExamCategoryChapterRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCategoryChapterCommand>(request);
            return await ExecuteAsync<UpdateExamCategoryChapterCommand, Unit>(command);
        }
    }
}
