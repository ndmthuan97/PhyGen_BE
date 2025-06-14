using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.ExamQuestions.Commands;
using PhyGen.Application.ExamQuestions.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixContentItems.Commands;
using PhyGen.Application.MatrixContentItems.Queries;
using PhyGen.Application.MatrixContentItems.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/matrixcontentitems")]
    [ApiController]
    public class MatrixContentItemController : BaseController<MatrixContentItemController>
    {
        public MatrixContentItemController(IMediator mediator, ILogger<MatrixContentItemController> logger)
            : base(mediator, logger) { }

        [HttpGet("{matrixContentItemId}")]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixContentItemResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByMatrixContentItemId(int matrixContentItemId)
        {
            var request = new GetMatrixContentItemByIdQuery(matrixContentItemId);
            return await ExecuteAsync<GetMatrixContentItemByIdQuery, MatrixContentItemResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MatrixContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateMatrixContentItemRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixContentItemCommand>(request);
            return await ExecuteAsync<CreateMatrixContentItemCommand, int>(command);
        }

        [HttpPut("{matrixContentItemId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(int matrixContentItemId, [FromBody] UpdateMatrixContentItemRequest request)
        {
            if (request == null)
                return HandleNullRequest();
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixContentItemCommand>(request);
            command.Id = matrixContentItemId;
            return await ExecuteAsync<UpdateMatrixContentItemCommand, Unit>(command);
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
