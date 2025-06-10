using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ExamCategoryChapters.Commands;
using PhyGen.Application.ExamCategoryChapters.Queries;
using PhyGen.Application.ExamCategoryChapters.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/examcategorychapters")]
    [ApiController]
    public class ExamCategoryChapterController : BaseController<ExamCategoryChapterController>
    {
        public ExamCategoryChapterController(IMediator mediator, ILogger<ExamCategoryChapterController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ExamCategoryChapterResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllExamCategoryChapters()
        {
            var request = new GetAllExamCategoryChaptersQuery();
            return await ExecuteAsync<GetAllExamCategoryChaptersQuery, List<ExamCategoryChapterResponse>>(request);
        }

        [HttpGet("{examCategoryChapterId}")]
        [ProducesResponseType(typeof(ApiResponse<ExamCategoryChapterResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamCategoryChapterById(Guid examCategoryChapterId)
        {
            var request = new GetExamCategoryChapterByIdQuery(examCategoryChapterId);
            return await ExecuteAsync<GetExamCategoryChapterByIdQuery, ExamCategoryChapterResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExamCategoryChapter([FromBody] CreateExamCategoryChapterRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCategoryChapterCommand>(request);
            return await ExecuteAsync<CreateExamCategoryChapterCommand, Guid>(command);
        }

        [HttpPut("{examCategoryChapterId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExamCategoryChapter(Guid examCategoryChapterId, [FromBody] UpdateExamCategoryChapterRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCategoryChapterCommand>(request);
            command.ExamCategoryChapterId = examCategoryChapterId;
            return await ExecuteAsync<UpdateExamCategoryChapterCommand, Unit>(command);
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
