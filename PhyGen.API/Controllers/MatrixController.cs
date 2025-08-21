using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Commands;
using PhyGen.Application.Matrices.Queries;
using PhyGen.Application.Matrices.Responses;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Specs;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;
using System.Security.Claims;

namespace PhyGen.API.Controllers
{
    [Route("api/matrices")]
    [ApiController]
    public class MatrixController : BaseController<MatrixController>
    {
        public MatrixController(IMediator mediator, ILogger<MatrixController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Pagination<MatrixResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllMatrices([FromQuery] MatrixSpecParam matrixSpecParam)
        {
            var query = new GetMatricesQuery(matrixSpecParam);
            return await ExecuteAsync<GetMatricesQuery, Pagination<MatrixResponse>>(query);
        }

        [HttpGet("{matrixId}")]
        [ProducesResponseType(typeof(ApiResponse<MatrixResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMatrixById(Guid matrixId)
        {
            var request = new GetMatrixByIdQuery(matrixId);
            return await ExecuteAsync<GetMatrixByIdQuery, MatrixResponse>(request);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MatrixResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMatrix([FromBody] CreateMatrixRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateMatrixCommand>(request);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new UserNotFoundException());
            command.CreatedBy = userEmail;

            return await ExecuteAsync<CreateMatrixCommand, MatrixResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrix([FromBody] UpdateMatrixRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixCommand>(request);
            return await ExecuteAsync<UpdateMatrixCommand, Unit>(command);
        }

        [HttpPut("{matrixId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMatrixFull([FromBody] UpdateMatrixFullCommand request)
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

            return await ExecuteAsync<UpdateMatrixFullCommand, Unit>(request);
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateMatrixStatusRequest request)
        {
            if (request == null || request.Ids == null || !request.Ids.Any())
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateMatrixStatusCommand>(request);
            return await ExecuteAsync<UpdateMatrixStatusCommand, Unit>(command);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMatrix([FromBody] DeleteMatrixRequest request)
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
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteMatrixCommand>(request);
            return await ExecuteAsync<DeleteMatrixCommand, Unit>(command);
        }
    }
}
