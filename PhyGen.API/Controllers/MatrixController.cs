using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models.Matrices;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrix.Commands;
using PhyGen.Application.Matrix.Queries;
using PhyGen.Application.Matrix.Responses;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/matrices")]
    [ApiController]

    public class MatrixController : BaseController<MatrixController>
    {
        public MatrixController(IMediator mediator, ILogger<MatrixController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<MatrixResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllMatrices()
        {
            var query = new GetAllMatricesQuery();
            return await ExecuteAsync<GetAllMatricesQuery, List<MatrixResponse>>(query);
        }

        [HttpGet("{matrixId}")]
        [ProducesResponseType(typeof(ApiResponse<MatrixResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixById(Guid matrixId)
        {
            var query = new GetMatrixByIdQuery(matrixId);
            return await ExecuteAsync<GetMatrixByIdQuery, MatrixResponse>(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrix([FromBody] CreateMatrixRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixCommand>(request);
            return await ExecuteAsync<CreateMatrixCommand, Guid>(command);
        }

        [HttpPut("{matrixId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrix(Guid matrixId, [FromBody] UpdateMatrixRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixCommand>(request);
            command.MatrixId = matrixId;

            return await ExecuteAsync<UpdateMatrixCommand, Unit>(command);
        }
    }
}
