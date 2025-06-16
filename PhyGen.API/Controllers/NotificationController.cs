using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Queries;
using PhyGen.Application.Notification.Responses;
using PhyGen.Domain.Entities;
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
        [ProducesResponseType(typeof(ApiResponse<List<NotificationResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllNotification()
        {
            var request = new GetAllNotificationQuery();
            return await ExecuteAsync<GetAllNotificationQuery, List<NotificationResponse>>(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            if (request == null)
                return HandleNullRequest();

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateNotificationCommand>(request);
            return await ExecuteAsync<CreateNotificationCommand, Guid>(command);
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
