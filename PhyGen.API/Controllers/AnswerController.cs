using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Answers.Commands;
using PhyGen.Application.Answers.Responses;
using PhyGen.Shared;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswerController : BaseController<AnswerController>
    {
        public AnswerController(IMediator mediator, ILogger<AnswerController> logger)
            : base(mediator, logger) { }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerCommand request)
        {
            return await ExecuteAsync<CreateAnswerCommand, Guid>(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAnswer(Guid id, [FromBody] UpdateAnswerCommand request)
        {
            request.Id = id;
            return await ExecuteAsync<UpdateAnswerCommand, Unit>(request);
        }
    }
}
