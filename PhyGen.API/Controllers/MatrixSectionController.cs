using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.MatrixSections.Commands;
using PhyGen.Application.MatrixSections.Queries;
using PhyGen.Application.MatrixSections.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/matrixsections")]
    [ApiController]
    public class MatrixSectionController : BaseController<MatrixSectionController>
    {
        public MatrixSectionController(IMediator mediator, ILogger<MatrixSectionController> logger)
            : base(mediator, logger) { }

        [HttpGet("{matrixSectionId}")]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixSectionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixSectionById(Guid matrixSectionId)
        {
            var query = new GetMatrixSectionByIdQuery(matrixSectionId);
            return await ExecuteAsync<GetMatrixSectionByIdQuery, List<MatrixSectionResponse>>(query);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixSectionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByMatrixId(Guid matrixId)
        {
            var query = new GetMatrixSectionsByMatrixIdQuery(matrixId);
            return await ExecuteAsync<GetMatrixSectionsByMatrixIdQuery, List<MatrixSectionResponse>>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MatrixSectionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrixSection([FromBody] CreateMatrixSectionRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixSectionCommand>(request);
            return await ExecuteAsync<CreateMatrixSectionCommand, MatrixSectionResponse>(command);
        }

        [HttpPut("{matrixSectionId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrixSection(Guid matrixSectionId, [FromBody] UpdateMatrixSectionRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixSectionCommand>(request);
            return await ExecuteAsync<UpdateMatrixSectionCommand, Unit>(command);
        }

        [HttpDelete("{matrixSectionId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid matrixSectionId)
        {
            var command = new DeleteMatrixSectionCommand(matrixSectionId);
            return await ExecuteAsync<DeleteMatrixSectionCommand, Unit>(command);
        }
    }
}
