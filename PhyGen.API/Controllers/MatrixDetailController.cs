using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixDetail.Commands;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;
using PhyGen.Application.MatrixDetail.Responses;
using PhyGen.Application.MatrixDetail.Queries;
using PhyGen.API.Models.MatrixDetail;
using PhyGen.API.Models.MatrixDetails;

namespace PhyGen.API.Controllers
{
    [Route("api/matrixdetails")]
    [ApiController]

    public class MatrixDetailController : BaseController<MatrixDetailController>
    {
        public MatrixDetailController(IMediator mediator, ILogger<MatrixDetailController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixDetailResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllMatrixDetails()
        {
            var query = new GetAllMatrixDetailsQuery();
            return await ExecuteAsync<GetAllMatrixDetailsQuery, List<MatrixDetailResponse>>(query);
        }

        [HttpGet("{matrixDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<MatrixDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixDetailById(Guid matrixDetailId)
        {
            var query = new GetMatrixDetailByIdQuery(matrixDetailId);
            return await ExecuteAsync<GetMatrixDetailByIdQuery, MatrixDetailResponse>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrixDetail([FromBody] CreateMatrixDetailRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields."]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixDetailCommand>(request);
            return await ExecuteAsync<CreateMatrixDetailCommand, Guid>(command);
        }

        [HttpPut("{matrixDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrixDetail(Guid matrixDetailId, [FromBody] UpdateMatrixDetailRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields."]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixDetailCommand>(request);
            command.MatrixDetailId = matrixDetailId;

            return await ExecuteAsync<UpdateMatrixDetailCommand, Unit>(command);
        }
    }
}
