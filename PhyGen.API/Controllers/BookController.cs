using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.Application.Mapping;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using System.Net;
using PhyGen.Application.Books.Responses;
using PhyGen.Application.Books.Queries;
using PhyGen.API.Models.Books;
using PhyGen.Application.Books.Commands;

namespace PhyGen.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : BaseController<BookController>
    {
        public BookController(IMediator mediator, ILogger<BookController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<BookResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBooks()
        {
            var request = new GetAllBooksQuery();
            return await ExecuteAsync<GetAllBooksQuery, List<BookResponse>>(request);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<BookResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBookById(Guid bookId)
        {
            var request = new GetBookByIdQuery(bookId);
            return await ExecuteAsync<GetBookByIdQuery, BookResponse>(request);
        }

        [HttpGet("series/{bookSeriesId}")]
        [ProducesResponseType(typeof(ApiResponse<List<BookResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooksByBookSeriesId(Guid chapterId)
        {
            var request = new GetBooksByBookSeriesIdQuery(chapterId);
            return await ExecuteAsync<GetBooksByBookSeriesIdQuery, List<BookResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateBookCommand>(request);
            return await ExecuteAsync<CreateBookCommand, Guid>(command);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBook(Guid BookId, [FromBody] UpdateBookRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateBookCommand>(request);
            command.BookId = BookId;
            return await ExecuteAsync<UpdateBookCommand, Unit>(command);
        }
    }
}
