using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixSectionDetails.Commands;
using PhyGen.Application.MatrixSectionDetails.Queries;
using PhyGen.Application.MatrixSectionDetails.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/matrixsectiondetails")]
    [ApiController]
    public class MatrixSectionDetailController : BaseController<MatrixSectionDetailController>
    {
        public MatrixSectionDetailController(IMediator mediator, ILogger<MatrixSectionDetailController> logger)
            : base(mediator, logger) { }

        [HttpGet("{matrixSectionDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<MatrixSectionDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixSectionDetailById(Guid id)
        {
            var query = new GetMatrixSectionDetailByIdQuery(id);
            return await ExecuteAsync<GetMatrixSectionDetailByIdQuery, MatrixSectionDetailResponse>(query);
        }

        [HttpGet("matrixsection/{matrixSectionId}")]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixSectionDetailResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixSectionDetailByMatrixSectionId(Guid matrixSectionId)
        {
            var query = new GetMatrixSectionDetailsByMatrixSectionIdQuery(matrixSectionId);
            return await ExecuteAsync<GetMatrixSectionDetailsByMatrixSectionIdQuery, List<MatrixSectionDetailResponse>>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MatrixSectionDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrixSectionDetail([FromBody] CreateMatrixSectionDetailRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixSectionDetailCommand>(request);
            return await ExecuteAsync<CreateMatrixSectionDetailCommand, MatrixSectionDetailResponse>(command);
        }

        [HttpPut("{matrixSectionDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrixSectionDetail(Guid matrixSectionDetailId, [FromBody] UpdateMatrixSectionDetailRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixSectionDetailCommand>(request);
            return await ExecuteAsync<UpdateMatrixSectionDetailCommand, Unit>(command);
        }

        [HttpDelete("{matrixSectionDetailId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid matrixSectionDetailId, [FromBody] DeleteMatrixSectionDetailRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteMatrixSectionDetailCommand>(request);
            return await ExecuteAsync<DeleteMatrixSectionDetailCommand, Unit>(command);
        }
    }
}
