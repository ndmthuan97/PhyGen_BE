using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
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
        [ProducesResponseType(typeof(ApiResponse<MatrixContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixContentItemById(Guid matrixContentItemId)
        {
            var request = new GetMatrixContentItemByIdQuery(matrixContentItemId);
            return await ExecuteAsync<GetMatrixContentItemByIdQuery, MatrixContentItemResponse>(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<MatrixContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixContentItemByMatrixIdAndContentItemId([FromQuery] Guid matrixId, [FromQuery] Guid contentItemId)
        {
            var request = new GetMatrixContentItemsByMatrixIdAndContentItemIdQuery(matrixId, contentItemId);
            return await ExecuteAsync<GetMatrixContentItemsByMatrixIdAndContentItemIdQuery, MatrixContentItemResponse>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MatrixContentItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrixContentItem([FromBody] CreateMatrixContentItemCommand request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixContentItemCommand>(request);
            return await ExecuteAsync<CreateMatrixContentItemCommand, MatrixContentItemResponse>(command);
        }

        [HttpPut("{matrixContentItemId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrixContentItem(Guid matrixContentItemId, [FromBody] UpdateMatrixContentItemCommand request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixContentItemCommand>(request);
            return await ExecuteAsync<UpdateMatrixContentItemCommand, Unit>(command);
        }
    }
}
