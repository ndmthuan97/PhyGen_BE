using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Exceptions;
using PhyGen.Application.Notification.Handlers;
using PhyGen.Application.Notification.Queries;
using PhyGen.Application.Notification.Responses;
using PhyGen.Application.Users.Dtos;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;

namespace PhyGen.API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : BaseController<NotificationController>
    {
        public NotificationController(IMediator mediator, ILogger<NotificationController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Pagination<NotificationResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllNotification(
        [FromQuery] Guid userId,
        [FromQuery] int pageIndex = 1)
        {
            var request = new GetAllNotificationQuery
            {
                Id = userId,
                PageIndex = pageIndex,
                PageSize = 10
            };

            return await ExecuteAsync<GetAllNotificationQuery, Pagination<NotificationResponse>>(request);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateNotificationCommand>(request);
            return await ExecuteAsync<CreateNotificationCommand, NotificationResponse>(command);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationRequest request)
            {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateNotificationCommand>(request);
            return await ExecuteAsync<UpdateNotificationCommand, Unit>(command);
            }

        [HttpDelete("{notificationId}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteExam(int notificationId)
        {
            var command = new DeleteNotificationCommand
            {
                Id = notificationId
            }; 
            return await ExecuteAsync<DeleteNotificationCommand, Unit>(command);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(
            [FromQuery] int Id,
            [FromQuery] Guid? UserId)
        {
            var filter = new SendNotificationCommand
            {
                Id = Id,    
                UserId = UserId
            };
            return Ok(new NotificationSend {});
        }
        [HttpPut("maskasread")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateReadNotification([FromQuery] Guid UserId)
        {
            var request = new UpdateNotificationRequest
            {
                UserId = UserId
            };           
            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateNotificationReadCommand>(request);
            return await ExecuteAsync<UpdateNotificationReadCommand, Unit>(command);
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
