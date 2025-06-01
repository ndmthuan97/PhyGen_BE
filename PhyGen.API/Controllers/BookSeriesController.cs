using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Application.BookSeries.Responses;
using PhyGen.Application.BookSeries.Queries;
using PhyGen.API.Models.BookSeries;
using PhyGen.Application.BookSeries.Commands;

namespace PhyGen.API.Controllers
{
    [Route("api/bookseries")]
    [ApiController]
    public class BookSeriesController : BaseController<BookSeriesController>
    {
        public BookSeriesController(IMediator mediator, ILogger<BookSeriesController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<BookSeriesResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBooks()
        {
            var request = new GetAllBookSeriesQuery();
            return await ExecuteAsync<GetAllBookSeriesQuery, List<BookSeriesResponse>>(request);
        }

        [HttpGet("{bookSeriesId}")]
        [ProducesResponseType(typeof(ApiResponse<BookSeriesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBookSeriesById(Guid BookSeriesId)
        {
            var request = new GetBookSeriesByIdQuery(BookSeriesId);
            return await ExecuteAsync<GetBookSeriesByIdQuery, BookSeriesResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookSeriesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBookSeries([FromBody] CreateBookSeriesRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateBookSeriesCommand>(request);
            return await ExecuteAsync<CreateBookSeriesCommand, Guid>(command);
        }

        [HttpPut("{bookSeriesId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBookSeries(Guid BookSeriesId, [FromBody] UpdateBookSeriesRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateBookSeriesCommand>(request);
            return await ExecuteAsync<UpdateBookSeriesCommand, Unit>(command);
        }
    }
}
