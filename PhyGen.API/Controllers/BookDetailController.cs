using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.BookDetails;
using PhyGen.Application.BookDetails.Commands;
using PhyGen.Application.BookDetails.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Application.BookDetails.Handlers;
using PhyGen.Application.BookDetails.Queries;

namespace PhyGen.API.Controllers
{
    [Route("api/bookdetails")]
    [ApiController]
    public class BookDetailController : BaseController<BookDetailController>
    {
        public BookDetailController(IMediator mediator, ILogger<BookDetailController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<BookDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBookDetailByBookIdAndChapterId([FromQuery] Guid bookId, [FromQuery] Guid chapterId)
        {
            var request = new GetBookDetailByBookIdAndChapterIdQuery(bookId, chapterId);
            return await ExecuteAsync<GetBookDetailByBookIdAndChapterIdQuery, BookDetailResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBookDetail([FromBody] CreateBookDetailRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateBookDetailCommand>(request);
            return await ExecuteAsync<CreateBookDetailCommand, Guid>(command);
        }

        [HttpPut("{BookDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBookDetail(Guid BookDetailId, [FromBody] UpdateBookDetailRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateBookDetailCommand>(request);
            return await ExecuteAsync<UpdateBookDetailCommand, Unit>(command);
        }
    }
}
